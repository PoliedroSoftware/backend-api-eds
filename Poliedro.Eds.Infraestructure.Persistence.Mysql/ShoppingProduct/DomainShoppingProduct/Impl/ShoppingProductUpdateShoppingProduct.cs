using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ShoppingProduct.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ShoppingProduct.DomainShopping.Impl;

public class ShoppingProductUpdateShoppingProduct(DataBaseContext context, IRedisService redisService) : IShoppingProductUpdateShoppingProduct
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(ShoppingProductEntity shoppingProductEntity)
    {
        if (!await EntityExists(shoppingProductEntity.IdShoppingProduct))
            return ShoppingProductErrorBuilder.ShoppingProductNotFoundException(shoppingProductEntity.IdShoppingProduct);

        context.ShoppingProduct.Update(shoppingProductEntity);

        if (await context.SaveChangesAsync() <= 0)
            return ShoppingProductErrorBuilder.ShoppingProductUpdateException();
        await redisService.RemoveByPrefixAsync("shoppingproduct:");

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.ShoppingProduct
            .AsNoTracking()
            .AnyAsync(c => c.IdShoppingProduct == id);
    }
}