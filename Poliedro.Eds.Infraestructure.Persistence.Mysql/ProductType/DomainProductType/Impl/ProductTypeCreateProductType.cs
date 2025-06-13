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

public class ProductTypeCreateProductType(DataBaseContext context,IRedisService redisService) : IProductTypeCreateProductType
{
    public async Task<Result<VoidResult, Error>> CreateAsync(ProductTypeEntity ProductTypeEntity)
    {
        await context.ProductType.AddAsync(ProductTypeEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return ProductTypeErrorBuilder.ProductTypeCreationException();
        await redisService.RemoveByPrefixAsync("productType:");
        return VoidResult.Instance;
    }
}
