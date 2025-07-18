using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Expenditures.DomainExpenditures;
using Poliedro.Eds.Domain.Expenditures.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Expenditures.DomainExpenditures.Impl;

public class ExpendituresGetAllExpenditures(
    ITenantDbContextFactory dbContextFactory
    ) : IExpendituresGetAllExpenditures
{
    public async Task<IEnumerable<ExpendituresEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.Expenditures
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return data;
    }
}
