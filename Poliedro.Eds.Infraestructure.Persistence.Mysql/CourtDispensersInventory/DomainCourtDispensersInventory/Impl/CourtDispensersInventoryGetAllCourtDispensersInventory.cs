using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.CourtDispensersInventory.DomainCourtDispensersInventory.Impl;

public class CourtDispensersInventoryGetAllCourtDispensersInventory(
    ITenantDbContextFactory dbContextFactory
    ) : ICourtDispensersInventoryGetAllCourtDispensersInventory
{
    public async Task<IEnumerable<CourtDispensersInventoryEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.CourtDispensersInventory
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
        .Take(paginationParams.PageSize)
        .ToListAsync();
        return data;
    }
}