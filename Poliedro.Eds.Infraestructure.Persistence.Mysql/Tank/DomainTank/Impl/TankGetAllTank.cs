using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Tank.DomainTank.Impl;

public class TankGetAllTank(
    ITenantDbContextFactory dbContextFactory
    ) : ITankGetAllTank
{
    public async Task<IEnumerable<TankEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.Tank
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
        .Take(paginationParams.PageSize)
        .ToListAsync();
        return data;
    }
}
