using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ShoppingProduct.DomainShopping.Impl;

public class ShoppingProductGetAllShoppingProduct(
    ITenantDbContextFactory dbContextFactory
    ) : IShoppingProductGetAllShoppingProduct
{
    public async Task<IEnumerable<ShoppingProductEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.ShoppingProduct
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return data;
    }
}