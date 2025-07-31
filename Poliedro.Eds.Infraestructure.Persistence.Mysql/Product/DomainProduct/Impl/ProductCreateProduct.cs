using Poliedro.Eds.Application.Court.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.DomainProduct;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Product.DomainProduct.Impl;

public class ProductCreateProduct(ITenantDbContextFactory dbContextFactory) : IProductCreateProduct
{
    public async Task<Result<VoidResult, Error>> CreateAsync(ProductEntity ProductEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.Product.AddAsync(ProductEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return CourtErrorBuilder.CourtCreationException();
        return VoidResult.Instance;
    }
}
