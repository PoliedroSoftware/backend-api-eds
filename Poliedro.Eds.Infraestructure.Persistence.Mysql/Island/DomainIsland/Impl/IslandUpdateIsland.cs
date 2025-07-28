using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Island.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Island.DomainIsland;
using Poliedro.Eds.Domain.Island.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Island.DomainIsland.Impl;

public class IslandUpdateIsland(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IIslandUpdateIsland
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(IslandEntity islandEntity)
    {
        if (!await EntityExists(islandEntity.IdIsland))
            return IslandErrorBuilder.IslandNotFoundException(islandEntity.IdIsland);

        using var context = dbContextFactory.CreateDbContext();
        context.Island.Update(islandEntity);

        if (await context.SaveChangesAsync() <= 0)
            return IslandErrorBuilder.IslandUpdateException();
        await redisService.RemoveByPrefixAsync("island:");

        return VoidResult.Instance;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Island
            .AsNoTracking()
            .AnyAsync(c => c.IdIsland == id);
    }
}
