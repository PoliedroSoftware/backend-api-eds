using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Compartiment.DomainCompartiment.Impl;

public class CompartimentGetAllService(
    ITenantDbContextFactory dbContextFactory
    ) : ICompartimentGetAllService
{
    public async Task<IEnumerable<CompartimentEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.Compartiment
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return data;
    }
}