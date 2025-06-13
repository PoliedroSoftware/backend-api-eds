using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ProductCompartiment.Errors;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ProductCompartiment.DomainProductCompartiment.Impl;

public class ProductCompartimentUpdateProductCompartiment(DataBaseContext context, IRedisService redisService) : IProductCompartimentUpdateProductCompartiment
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(ProductCompartimentEntity ProductCompartimentEntity)
    {
        if (!await EntityExists(ProductCompartimentEntity.IdProductCompartiment))
            return ProductCompartimentErrorBuilder.ProductCompartimentNotFoundException(ProductCompartimentEntity.IdProductCompartiment);

        context.ProductCompartiment.Update(ProductCompartimentEntity);

        if (await context.SaveChangesAsync() <= 0)
            return ProductCompartimentErrorBuilder.ProductCompartimentUpdateException();
        await redisService.RemoveByPrefixAsync("productCompartiment:");
        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.ProductCompartiment
            .AsNoTracking()
            .AnyAsync(c => c.IdProductCompartiment == id);
    }
}
