using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Tank.DomainTank.Impl;

public class TankGetAllTank(DataBaseContext context, IRedisService redisService) : ITankGetAllTank
{
    public async Task<IEnumerable<TankEntity>> GetAllAsync(PaginationParams paginationParams)
    {

        var totalRows = await context.Tank.CountAsync();

        string cacheKey = $"tank:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<TankEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.Tank
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
        .Take(paginationParams.PageSize)
        .ToListAsync();

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}