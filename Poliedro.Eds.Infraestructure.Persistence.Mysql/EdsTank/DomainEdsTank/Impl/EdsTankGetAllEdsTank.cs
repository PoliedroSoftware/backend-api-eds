using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;
using Poliedro.Eds.Domain.EdsTank.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EdsTank.DomainEdsTank.Impl;

public class EdsTankGetAllEdsTank(
    ITenantDbContextFactory dbContextFactory
    ) : IEdsTankGetAllEdsTank
{
    public async Task<IEnumerable<EdsTankEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();        
        var data = await context.EdsTank
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return data;
    }
}
