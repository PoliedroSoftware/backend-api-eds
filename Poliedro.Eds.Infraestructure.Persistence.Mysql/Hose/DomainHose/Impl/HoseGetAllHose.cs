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

        var query = from hose in context.Hose
                    join dispenser in context.Dispensers on hose.IdDispensers equals dispenser.Id
                    join productType in context.ProductTypes on hose.IdProductType equals productType.IdProductType
                    join eds in context.Eds on dispenser.EdsId equals eds.IdEds
                    join product in context.Product on hose.IdProductType equals product.IdProductType
                    where product.Date == context.Product
                    .Where(p => p.IdProductType == hose.IdProductType)
                    .Max(p => p.Date)

                    select new HoseDto(
                        hose.IdHose,
                        hose.Number,
                        hose.IdDispensers,
                        hose.AccumulatedGallons,
                        hose.AccumulatedAmount,
                        hose.IdProductType,
                        product.Price,
                        dispenser,
                        productType,
                        eds
                    );

        var hoseDtos = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        await redisService.SetCacheAsync(cacheKey, hoseDtos, TimeSpan.FromMinutes(1440));

        return hoseDtos;
    }

}