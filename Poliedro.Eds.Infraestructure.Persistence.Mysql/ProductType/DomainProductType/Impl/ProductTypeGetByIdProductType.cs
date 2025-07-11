using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ProductType.Errors;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductType.DomainProductType;
using Poliedro.Eds.Domain.ProductType.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ProductType.DomainProductType.Impl;

public class ProductTypeGetByIdProductType(DataBaseContext context,IRedisService redisService) : IProductTypeGetByIdProductType
{
    public async Task<Result<ProductTypeEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"productype:{id}";
        var cachedData = await redisService.GetCacheAsync<ProductTypeEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return ProductTypeErrorBuilder.ProductTypeNotFoundException(id);

        var data = await context.ProductType
            .FirstAsync(c => c.IdProductType == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        return await context.ProductType
            .AsNoTracking()
            .AnyAsync(c => c.IdProductType == id);
    }

}
