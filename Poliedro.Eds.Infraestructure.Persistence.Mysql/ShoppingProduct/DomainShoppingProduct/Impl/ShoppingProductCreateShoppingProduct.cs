using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ShoppingProduct.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ShoppingProduct.DomainShoppingProduct.Impl;

public class ShoppingProductCreateShoppingProduct(DataBaseContext context, IRedisService redisService) : IShoppingProductCreateShoppingProduct
{
    public async Task<Result<VoidResult, Error>> CreateAsync(ShoppingProductEntity shoppingProductEntity)
    {
        await context.ShoppingProduct.AddAsync(shoppingProductEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return ShoppingProductErrorBuilder.ShoppingProductCreationException();
        await redisService.RemoveByPrefixAsync("shoppingproduct:");

        return VoidResult.Instance;
    }
}