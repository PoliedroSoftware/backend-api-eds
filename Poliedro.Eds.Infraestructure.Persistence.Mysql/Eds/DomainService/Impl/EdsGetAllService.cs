using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Eds.DomainEds.Impl;

public class EdsGetAllService(DataBaseContext context, IRedisService redisService) : IEdsGetAllService
{
    public async Task<IEnumerable<EdsEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        var totalRows = await context.Eds.CountAsync();
        string cacheKey = $"eds:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<EdsEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.Eds
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();


        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}