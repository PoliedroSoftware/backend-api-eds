using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Product.Errors;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.DomainProduct;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Product.DomainProduct.Impl;

public class ProductGetAllProduct(DataBaseContext context,IRedisService redisService) : IProductGetAllProduct
{

    public async Task<IEnumerable<ProductEntity>> GetAllAsync(PaginationParams paginationParams)
    {
         var totalRows = await context.Product.CountAsync();

        string cacheKey = $"product:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<ProductEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;
        
        var data = await context.Product
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

       
        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}
