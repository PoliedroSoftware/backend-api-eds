using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Dtos;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl;

public class HoseGetAllHose(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor
    ) : IHoseGetAllHose
{
    public async Task<IEnumerable<HoseDto>> GetAllAsync(PaginationParams paginationParams)
    {

        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        string cacheKey = $"hose:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";

        var cachedDtos = await redisService.GetCacheAsync<IEnumerable<HoseDto>>(cacheKey);
        if (cachedDtos is not null)
            return cachedDtos;

        using var context = dbContextFactory.CreateDbContext();

        var validIds = await context.Hose.Select(h => h.IdHose).ToListAsync();

        var query = from hose in context.Hose
                    where validIds.Contains(hose.IdHose)
                    join dispenser in context.Dispensers on hose.IdDispensers equals dispenser.Id
                    join productType in context.ProductTypes on hose.IdProductType equals productType.IdProductType
                    join eds in context.Eds on dispenser.EdsId equals eds.IdEds
                    join product in context.Product on hose.IdProductType equals product.IdProductType
                    join compartiment in context.Compartiment on hose.IdCompartiment equals compartiment.IdCompartment
                    group new { hose, dispenser, productType, eds, product, compartiment } by hose.IdHose into g
                    select new HoseDto(
                        g.First().hose.IdHose,
                        g.First().hose.Number,
                        g.First().hose.IdDispensers,
                        g.First().hose.AccumulatedGallons,
                        g.First().hose.AccumulatedAmount,
                        g.First().hose.IdProductType,
                        g.First().product.Price,
                        g.First().hose.IdCompartiment,
                        g.First().dispenser,
                        g.First().productType,
                        g.First().eds
                    );

        var hoseDtos = (await query
            .ToListAsync())
            .GroupBy(h => h.IdHose)
            .Select(g => g.First())
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToList();


        await redisService.SetCacheAsync(cacheKey, hoseDtos, TimeSpan.FromMinutes(1440));

        return hoseDtos;
    }

}
