using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Island.DomainIsland;
using Poliedro.Eds.Domain.Island.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Island.Domainisland.Impl;

public class IslandGetAllIsland(DataBaseContext context,IRedisService redisService) : IIslandGetAllIsland
{
    public async Task<IEnumerable<IslandEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        var totalRows = await context.Island.CountAsync();

        string cacheKey = $"island:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<IslandEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.Island
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}