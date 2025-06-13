using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Island.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Island.DomainIsland;
using Poliedro.Eds.Domain.Island.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Island.Domainisland.Impl;

public class IslandGetByIdIsland(DataBaseContext context,IRedisService redisService) : IIslandGetByIdIsland
{
    public async Task<Result<IslandEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"island:{id}";
        var cachedData = await redisService.GetCacheAsync<IslandEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return IslandErrorBuilder.IslandNotFoundException(id);
        var data = await context.Island
            .FirstAsync(c => c.IdIsland == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        return await context.Island
            .AsNoTracking()
            .AnyAsync(c => c.IdIsland == id);
    }
}