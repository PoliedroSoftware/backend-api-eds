using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Dtos;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl;

public class HoseGetAllHose(DataBaseContext context, IRedisService redisService) : IHoseGetAllHose
{
    public async Task<IEnumerable<HoseDto>> GetAllAsync(PaginationParams paginationParams)
    {
        string cacheKey = $"hoseDto:{paginationParams.PageNumber}:{paginationParams.PageSize}";

        var cachedDtos = await redisService.GetCacheAsync<IEnumerable<HoseDto>>(cacheKey);
        if (cachedDtos is not null)
            return cachedDtos;

        var query = from hose in context.Hose
                    join dispenser in context.Dispensers on hose.IdDispensers equals dispenser.Id
                    join productType in context.ProductTypes on hose.IdProductType equals productType.IdProductType
                    join eds in context.Eds on dispenser.EdsId equals eds.IdEds
                    select new HoseDto(
                        hose.IdHose,
                        hose.Number,
                        hose.IdDispensers,
                        hose.AccumulatedGallons,
                        hose.AccumulatedAmount,
                        hose.IdProductType,
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