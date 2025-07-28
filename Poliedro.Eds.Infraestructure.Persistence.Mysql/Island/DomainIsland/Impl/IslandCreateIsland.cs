using Poliedro.Eds.Application.Island.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Island.DomainIsland;
using Poliedro.Eds.Domain.Island.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Island.Domainisland.Impl;
public class IslandCreateIsland(ITenantDbContextFactory dbContextFactory) : IIslandCreateIsland
{
    public async Task<Result<VoidResult, Error>> CreateAsync(IslandEntity islandEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.Island.AddAsync(islandEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return IslandErrorBuilder.IslandCreationException();
        return VoidResult.Instance;
    }
}