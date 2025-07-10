using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Court.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Product.Errors;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.DomainProduct;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Product.DomainProduct.Impl;

public class ProductGetByIdProduct(ITenantDbContextFactory dbContextFactory,IRedisService redisService) : IProductGetByIdProduct
{
    public async Task<Result<ProductEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"product:{id}";
        var cachedData = await redisService.GetCacheAsync<ProductEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return ProductErrorBuilder.ProductNotFoundException(id);

        using var context = dbContextFactory.CreateDbContext();
        var data = await context.Product
            .FirstAsync(c => c.IdProduct == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Product
            .AsNoTracking()
            .AnyAsync(c => c.IdProduct == id);
    }

}
