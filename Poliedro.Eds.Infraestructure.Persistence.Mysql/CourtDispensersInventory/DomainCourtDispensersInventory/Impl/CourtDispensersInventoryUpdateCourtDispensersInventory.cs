using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.CourtDispensersInventory.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.CourtDispensersInventory.DomainCourtDispensersInventory.Impl;

public class CourtDispensersInventoryUpdateCourtDispensersInventory(DataBaseContext context, IRedisService redisService) : ICourtDispensersInventoryUpdateCourtDispensersInventory
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(CourtDispensersInventoryEntity courtdispensersinventoryEntity)
    {
        if (!await EntityExists(courtdispensersinventoryEntity.IdCourtDispensersInventory))
            return CourtDispensersInventoryErrorBuilder.CourtDispensersInventoryNotFoundException(courtdispensersinventoryEntity.IdCourtDispensersInventory);

        context.CourtDispensersInventory.Update(courtdispensersinventoryEntity);

        if (await context.SaveChangesAsync() <= 0)
            return CourtDispensersInventoryErrorBuilder.CourtDispensersInventoryUpdateException();
        await redisService.RemoveByPrefixAsync("courtdispensersinventory:");

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.CourtDispensersInventory
            .AsNoTracking()
            .AnyAsync(c => c.IdCourtDispensersInventory == id);
    }
}