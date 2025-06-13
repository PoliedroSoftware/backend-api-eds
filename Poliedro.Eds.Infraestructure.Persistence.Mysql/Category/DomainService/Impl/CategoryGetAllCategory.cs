using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Category.DomainCategory.Impl;

public class CategoryGetAllCategory(DataBaseContext context, IRedisService redisService) : ICategoryGetAllCategory
{
    public async Task<IEnumerable<CategoryEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        var totalRows = await context.Category.CountAsync();
        string cacheKey = $"category:{paginationParams.PageNumber}:{paginationParams.PageSize}";
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