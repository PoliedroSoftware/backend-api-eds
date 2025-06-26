using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ProductCompartiment.Errors;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ProductCompartiment.DomainProductCompartiment.Impl;

public class ProductCompartimentGetByIdProductCompartiment(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IProductCompartimentGetByIdProductCompartiment
{
    public async Task<Result<ProductCompartimentEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"productCompartiment:{id}";
        var cachedData = await redisService.GetCacheAsync<ProductCompartimentEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return ProductCompartimentErrorBuilder.ProductCompartimentNotFoundException(id);

        using var context = dbContextFactory.CreateDbContext();

        var data = await context.ProductCompartiment
            .FirstAsync(c => c.IdProductCompartiment == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Tank
            .AsNoTracking()
            .AnyAsync(c => c.IdTank == id);
    }

}
