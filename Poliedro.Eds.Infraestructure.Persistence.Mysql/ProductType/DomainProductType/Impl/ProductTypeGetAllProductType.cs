using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.ProductType.DomainProductType;
using Poliedro.Eds.Domain.ProductType.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.ProductType.DomainProductType.Impl;

public class ProductTypeGetAllProductType(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor
    ) : IProductTypeGetAllProductType
{

    public async Task<IEnumerable<ProductTypeEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        var totalRows = await context.ProductType.CountAsync();

        string cacheKey = $"productType:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<ProductTypeEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;
        
        var data = await context.ProductType
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;    
    }
}
