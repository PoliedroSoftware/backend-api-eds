using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Category.DomainCategory.Impl;

public class CategoryGetAllService(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor
    ) : ICategoryGetAllService
{
    public async Task<IEnumerable<CategoryEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var totalRows = await context.Category.CountAsync();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();

        string cacheKey = $"category:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<CategoryEntity>>(cacheKey);
        
        if (cachedData is not null) 
            return cachedData;

        var data = await context.Category
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();


        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}