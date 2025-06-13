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

public class ProductUpdateProduct(DataBaseContext context, IRedisService redisService) : IProductUpdateProduct
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(ProductEntity ProductEntity)
    {
        if (!await EntityExists(ProductEntity.IdProduct))
            return CourtErrorBuilder.CourtNotFoundException(ProductEntity.IdProduct);

        context.Product.Update(ProductEntity);

        if (await context.SaveChangesAsync() <= 0)
            return CourtErrorBuilder.CourtUpdateException();
        await redisService.RemoveByPrefixAsync("product:");
        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Product
            .AsNoTracking()
            .AnyAsync(c => c.IdProduct == id);
    }
}
