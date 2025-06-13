using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.CourtDispensersInventory.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.CourtDispensersInventory.DomainCourtDispensersInventory.Impl;

public class CourtDispensersInventoryGetByIdCourtDispensersInventory(DataBaseContext context, IRedisService redisService) : ICourtDispensersInventoryGetByIdCourtDispensersInventory
{
    public async Task<Result<CourtDispensersInventoryEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"courtdispensersinventory:{id}";
        var cachedData = await redisService.GetCacheAsync<CourtDispensersInventoryEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return CourtDispensersInventoryErrorBuilder.CourtDispensersInventoryNotFoundException(id);
        var data = await context.CourtDispensersInventory
            .FirstAsync(c => c.IdCourtDispensersInventory == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        return await context.CourtDispensersInventory
            .AsNoTracking()
            .AnyAsync(c => c.IdCourtDispensersInventory == id);
    }
}