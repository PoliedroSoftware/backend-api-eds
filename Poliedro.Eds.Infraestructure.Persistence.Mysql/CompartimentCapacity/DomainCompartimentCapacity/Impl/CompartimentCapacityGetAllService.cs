using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.CompartimentCapacity.DomainCompartimentCapacity.Impl;

public class CompartimentCapacityGetAllService(
    ITenantDbContextFactory dbContextFactory
    ) : ICompartimentCapacityGetAllService
{

    public async Task<IEnumerable<CompartimentCapacityEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.CompartimentCapacity
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return data;
    }
}
