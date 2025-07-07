using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.CourtDispensersInventory.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.CourtDispensersInventory.DomainCourtDispensersInventory.Impl;

public class CourtDispensersInventoryUpdateCourtDispensersInventory(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : ICourtDispensersInventoryUpdateCourtDispensersInventory
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(CourtDispensersInventoryEntity courtdispensersinventoryEntity)
    {
        if (!await EntityExists(courtdispensersinventoryEntity.IdCourtDispensersInventory))
            return CourtDispensersInventoryErrorBuilder.CourtDispensersInventoryNotFoundException(courtdispensersinventoryEntity.IdCourtDispensersInventory);

        using var context = dbContextFactory.CreateDbContext();
        context.CourtDispensersInventory.Update(courtdispensersinventoryEntity);

        if (await context.SaveChangesAsync() <= 0)
            return CourtDispensersInventoryErrorBuilder.CourtDispensersInventoryUpdateException();
        await redisService.RemoveByPrefixAsync("courtdispensersinventory:");

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.CourtDispensersInventory
            .AsNoTracking()
            .AnyAsync(c => c.IdCourtDispensersInventory == id);
    }
}