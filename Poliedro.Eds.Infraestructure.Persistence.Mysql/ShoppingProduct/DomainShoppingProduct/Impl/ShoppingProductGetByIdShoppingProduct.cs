using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ShoppingProduct.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ShoppingProduct.DomainShopping.Impl;

public class ShoppingProductGetByIdShoppingProduct(DataBaseContext context,IRedisService redisService) : IShoppingProductGetByIdShoppingProduct
{
    public async Task<Result<ShoppingProductEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"shoppingproduct:{id}";
        var cachedData = await redisService.GetCacheAsync<ShoppingProductEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return ShoppingProductErrorBuilder.ShoppingProductNotFoundException(id);

        var data = await context.ShoppingProduct
            .FirstAsync(c => c.IdShoppingProduct == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.ShoppingProduct
            .AsNoTracking()
            .AnyAsync(c => c.IdShoppingProduct == id);
    }
}