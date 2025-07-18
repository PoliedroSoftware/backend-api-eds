using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;

public class BusinessGetAllService(
    ITenantDbContextFactory dbContextFactory
    ) : IBusinessGetAllService
{
    public async Task<IEnumerable<BusinessEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var totalRows = await context.Business.CountAsync();
        var data = await context.Business
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return data;
    }
}