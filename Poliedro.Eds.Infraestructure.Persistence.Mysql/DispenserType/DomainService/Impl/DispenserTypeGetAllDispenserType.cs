using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DispenserType.DomainDispenserType;
using Poliedro.Eds.Domain.DispenserType.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.DispenserType.DomainDispenserType.Impl;

public class DispenserTypeGetAllDispenserType(
    ITenantDbContextFactory dbContextFactory
    ) : IDispenserTypeGetAllDispenserType
{
    public async Task<IEnumerable<DispenserTypeEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.DispenserType
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return data;
    }
}