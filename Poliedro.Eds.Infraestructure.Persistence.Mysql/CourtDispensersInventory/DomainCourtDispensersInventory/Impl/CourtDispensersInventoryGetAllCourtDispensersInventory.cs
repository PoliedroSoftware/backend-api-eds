using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.CourtDispensersInventory.DomainCourtDispensersInventory.Impl;

public class CourtDispensersInventoryGetAllCourtDispensersInventory(DataBaseContext context, IRedisService redisService) : ICourtDispensersInventoryGetAllCourtDispensersInventory
{
    public async Task<IEnumerable<CourtDispensersInventoryEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        var totalRows = await context.CourtDispensersInventory.CountAsync();

        string cacheKey = $"courtdispersersinventory:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<CourtDispensersInventoryEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.CourtDispensersInventory
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
        .Take(paginationParams.PageSize)
        .ToListAsync();

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}