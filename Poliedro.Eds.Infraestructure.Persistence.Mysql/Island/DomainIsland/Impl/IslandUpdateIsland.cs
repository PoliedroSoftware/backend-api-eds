using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Island.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Island.DomainIsland;
using Poliedro.Eds.Domain.Island.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Island.DomainIsland.Impl;

public class IslandUpdateIsland(DataBaseContext context, IRedisService redisService) : IIslandUpdateIsland
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(IslandEntity islandEntity)
    {
        if (!await EntityExists(islandEntity.IdIsland))
            return IslandErrorBuilder.IslandNotFoundException(islandEntity.IdIsland);

        context.Island.Update(islandEntity);

        if (await context.SaveChangesAsync() <= 0)
            return IslandErrorBuilder.IslandUpdateException();
        await redisService.RemoveByPrefixAsync("island:");

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Island
            .AsNoTracking()
            .AnyAsync(c => c.IdIsland == id);
    }
}