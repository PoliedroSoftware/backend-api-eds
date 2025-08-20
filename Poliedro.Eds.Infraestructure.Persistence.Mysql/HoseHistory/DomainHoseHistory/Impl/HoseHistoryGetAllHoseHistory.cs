using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;
using Poliedro.Eds.Domain.HoseHistory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.HoseHistory.DomainHoseHistory.Impl;

public class HoseHistoryGetAllHoseHistory(
    ITenantDbContextFactory dbContextFactory
    ) : IHoseHistoryGetAllHoseHistory
{
    public async Task<IEnumerable<HoseHistoryEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.HoseHistory
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
        .Take(paginationParams.PageSize)
        .ToListAsync();
        return data;
    }
}
