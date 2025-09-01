using Poliedro.Eds.Application.ProductCompartiment.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ProductCompartiment.DomainProductCompartiment.Impl;

public class ProductCompartimentCreateProductCompartiment(ITenantDbContextFactory dbContextFactory) : IProductCompartimentCreateProductCompartiment
{
    public async Task<Result<VoidResult, Error>> CreateAsync(ProductCompartimentEntity ProductCompartimentEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.ProductCompartiment.AddAsync(ProductCompartimentEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return ProductCompartimentErrorBuilder.ProductCompartimentCreationException();
        return VoidResult.Instance;
    }
}
