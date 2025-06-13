using Poliedro.Eds.Application.CourtDispensersInventory.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.CourtDispensersInventory.DomainCourtDispensersInventory.Impl;

public class CourtDispensersInventoryCreateCourtDispensersInventory(DataBaseContext context, IRedisService redisService) : ICourtDispensersInventoryCreateCourtDispensersInventory
{
    public async Task<Result<VoidResult, Error>> CreateAsync(CourtDispensersInventoryEntity courtdispensersinventoryEntity)
    {
        await context.CourtDispensersInventory.AddAsync(courtdispensersinventoryEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return CourtDispensersInventoryErrorBuilder.CourtDispensersInventoryCreationException();
        await redisService.RemoveByPrefixAsync("courtdispensersinventory:");

        return VoidResult.Instance;
    }
}