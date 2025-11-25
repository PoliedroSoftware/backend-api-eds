using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.DomainPosOfSaleDetails;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.PosOfSaleDetails.DomainPosOfSaleDetails.Impl;

public class PosOfSaleDetailsGetAllService(
    ITenantDbContextFactory dbContextFactory
    ) : IPosOfSaleDetailsGetAllService
{
    public async Task<Result<IEnumerable<PosOfSaleDetailsEntity>, Error>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.PosOfSaleDetails
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return data;
    }
}
