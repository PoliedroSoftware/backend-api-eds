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

public class ProductCompartimentCreateProductCompartiment(DataBaseContext context, IRedisService redisService) : IProductCompartimentCreateProductCompartiment
{
    public async Task<Result<VoidResult, Error>> CreateAsync(ProductCompartimentEntity ProductCompartimentEntity)
    {
        await context.ProductCompartiment.AddAsync(ProductCompartimentEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return ProductCompartimentErrorBuilder.ProductCompartimentCreationException();
        await redisService.RemoveByPrefixAsync("productCompartiment:");
        return VoidResult.Instance;
    }
}
