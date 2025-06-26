using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ProductType.Errors;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductType.DomainProductType;
using Poliedro.Eds.Domain.ProductType.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ProductType.DomainProductType.Impl;

public class ProductTypeUpdateProductType(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IProductTypeUpdateProductType
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(ProductTypeEntity ProductTypeEntity)
    {
        if (!await EntityExists(ProductTypeEntity.IdProductType))
            return ProductTypeErrorBuilder.ProductTypeNotFoundException(ProductTypeEntity.IdProductType);

        using var context = dbContextFactory.CreateDbContext();
        context.ProductType.Update(ProductTypeEntity);

        if (await context.SaveChangesAsync() <= 0)
            return ProductTypeErrorBuilder.ProductTypeUpdateException();
        await redisService.RemoveByPrefixAsync("productType:");
        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.ProductType
            .AsNoTracking()
            .AnyAsync(c => c.IdProductType == id);
    }
}
