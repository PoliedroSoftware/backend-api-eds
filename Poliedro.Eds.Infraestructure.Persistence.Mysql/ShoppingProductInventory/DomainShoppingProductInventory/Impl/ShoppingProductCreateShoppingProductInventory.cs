using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ShoppingProduct.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ShoppingProductInventory.DomainShoppingProductInventory.Impl;

public class ShoppingProductCreateShoppingProductInventory(
    ITenantDbContextFactory dbContextFactory, 
    IRedisService redisService) : IShoppingCreateShoppingProductInventory
{
    public async Task<Result<VoidResult, Error>> CreateAsync(ShoppingProductInventoryEntity shoppingProductEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.ShoppingProductInventory.AddAsync(shoppingProductEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return ShoppingProductErrorBuilder.ShoppingProductCreationException();
        await redisService.RemoveByPrefixAsync("shoppingProductInventory:");

        return VoidResult.Instance;
    }
}
