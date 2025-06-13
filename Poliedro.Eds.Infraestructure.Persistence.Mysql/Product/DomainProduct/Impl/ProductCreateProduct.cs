using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Court.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.DomainProduct;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Product.DomainProduct.Impl;

public class ProductCreateProduct(DataBaseContext context, IRedisService redisService) : IProductCreateProduct
{
    public async Task<Result<VoidResult, Error>> CreateAsync(ProductEntity ProductEntity)
    {
        await context.Product.AddAsync(ProductEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return CourtErrorBuilder.CourtCreationException();
        await redisService.RemoveByPrefixAsync("product:");
        return VoidResult.Instance;
    }
}
