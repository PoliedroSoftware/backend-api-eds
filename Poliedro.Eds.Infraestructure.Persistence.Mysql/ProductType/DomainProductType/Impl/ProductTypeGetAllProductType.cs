using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.ProductType.DomainProductType;
using Poliedro.Eds.Domain.ProductType.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ProductType.DomainProductType.Impl;

public class ProductTypeGetAllProductType(
    ITenantDbContextFactory dbContextFactory
    ) : IProductTypeGetAllProductType
{

    public async Task<IEnumerable<ProductTypeEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.ProductType
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return data;
    }
}
