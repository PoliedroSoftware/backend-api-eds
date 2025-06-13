using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Dispensers.DomainDispensers;
using Poliedro.Eds.Domain.Dispensers.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Dispensers.DomainDispensers.Impl;

public class DispensersGetAllDispensers(DataBaseContext context, IRedisService redisService) : IDispensersGetAllDispensers
{
    public async Task<IEnumerable<DispensersEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        var totalRows = await context.Dispensers.CountAsync();

        string cacheKey = $"dispensers:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<DispensersEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.Dispensers
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();


        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}