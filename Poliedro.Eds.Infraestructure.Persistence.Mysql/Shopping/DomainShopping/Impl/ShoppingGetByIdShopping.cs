using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Shopping.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Shopping.DomainShopping.Impl;

public class ShoppingGetByIdShopping(DataBaseContext context, IRedisService redisService) : IShoppingGetByIdShopping
{
    public async Task<Result<ShoppingEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"shopping:{id}";
        var cachedData = await redisService.GetCacheAsync<ShoppingEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return ShoppingErrorBuilder.ShoppingNotFoundException(id);

        var data = await context.Shopping
            .FirstAsync(c => c.IdShopping == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Shopping
            .AsNoTracking()
            .AnyAsync(c => c.IdShopping == id);
    }
}