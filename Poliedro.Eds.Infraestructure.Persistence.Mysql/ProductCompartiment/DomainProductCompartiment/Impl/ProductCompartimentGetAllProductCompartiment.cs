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

public class ProductCompartimentGetAllProductCompartiment(DataBaseContext context,IRedisService redisService) : IProductCompartimentGetAllProductCompartiment
{

    public async Task<IEnumerable<ProductCompartimentEntity>> GetAllAsync(PaginationParams paginationParams)
    {
         var totalRows = await context.ProductCompartiment.CountAsync();

        string cacheKey = $"productCompartiment:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<ProductCompartimentEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;
        
        var data = await context.ProductCompartiment
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

       
        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}
