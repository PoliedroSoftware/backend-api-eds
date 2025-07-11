using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Shopping.DomainShopping.Impl;
public class ShoppingGetAllShopping(DataBaseContext context, IRedisService redisService) : IShoppingGetAllShopping
{
    public async Task<IEnumerable<ShoppingEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        var totalRows = await context.Shopping.CountAsync();
        string cacheKey = $"shopping:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<ShoppingEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.Shopping
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();


        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}