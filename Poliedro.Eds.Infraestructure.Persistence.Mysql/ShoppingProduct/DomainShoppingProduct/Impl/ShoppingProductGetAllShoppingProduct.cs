using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ShoppingProduct.DomainShopping.Impl;
public class ShoppingProductGetAllShoppingProduct(DataBaseContext context, IRedisService redisService) : IShoppingProductGetAllShoppingProduct
{
    public async Task<IEnumerable<ShoppingProductEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        var totalRows = await context.ShoppingProduct.CountAsync();
        string cacheKey = $"shoppingproduct:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<ShoppingProductEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.ShoppingProduct
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();


        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}