using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Category.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Category.DomainCategory.Impl;

public class CategoryGetByIdService(
    ITenantDbContextFactory dbContextFactory, 
    IRedisService redisService,
    ILogger<CategoryGetByIdService> logger) : ICategoryGetByIdService
{
    public async Task<Result<CategoryEntity, Error>> GetByIdAsync(int id)
    {
        logger.LogInformation("Getting category by ID: {CategoryId}", id);
        
        string cacheKey = $"category:{id}";
        var cachedData = await redisService.GetCacheAsync<CategoryEntity>(cacheKey);

        if (cachedData is not null)
        {
            logger.LogInformation("Category found in cache: {CategoryId}", id);
            return cachedData;
        }

        if (!await EntityExists(id))
        {
            logger.LogWarning("Category not found: {CategoryId}", id);
            return CategoryErrorBuilder.CategoryNotFoundException(id);
        }

        using var context = dbContextFactory.CreateDbContext();

        var data = await context.Category
            .FirstAsync(c => c.IdCategory == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));
        
        logger.LogInformation("Successfully retrieved category: {CategoryId} - {CategoryDescription}", id, data.Description);

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Category
            .AsNoTracking()
            .AnyAsync(c => c.IdCategory == id);
    }
}
