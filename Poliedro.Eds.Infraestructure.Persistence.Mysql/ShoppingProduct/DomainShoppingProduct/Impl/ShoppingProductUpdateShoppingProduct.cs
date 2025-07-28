using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ShoppingProduct.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ShoppingProduct.DomainShopping.Impl;

public class ShoppingProductUpdateShoppingProduct(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IShoppingProductUpdateShoppingProduct
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(ShoppingProductEntity shoppingProductEntity)
    {
        if (!await EntityExists(shoppingProductEntity.IdShoppingProduct))
            return ShoppingProductErrorBuilder.ShoppingProductNotFoundException(shoppingProductEntity.IdShoppingProduct);

        using var context = dbContextFactory.CreateDbContext();
        context.ShoppingProduct.Update(shoppingProductEntity);

        if (await context.SaveChangesAsync() <= 0)
            return ShoppingProductErrorBuilder.ShoppingProductUpdateException();
        await redisService.RemoveByPrefixAsync("shoppingproduct:");

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.ShoppingProduct
            .AsNoTracking()
            .AnyAsync(c => c.IdShoppingProduct == id);
    }
}
