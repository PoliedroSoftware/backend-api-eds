using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Category.Errors;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Category.DomainCategory.Impl;

public class CategoryCreateService(
    ITenantDbContextFactory dbContextFactory,
    ILogger<CategoryCreateService> logger) : ICategoryCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(CategoryEntity categoryEntity)
    {
        logger.LogInformation("Creating new category: {CategoryDescription}", categoryEntity.Description);
        
        using var context = dbContextFactory.CreateDbContext();
        await context.Category.AddAsync(categoryEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
        {
            logger.LogError("Failed to save category to database: {CategoryDescription}", categoryEntity.Description);
            return CategoryErrorBuilder.CategoryCreationException();
        }
        
        logger.LogInformation("Successfully created category: {CategoryDescription}", categoryEntity.Description);
        return VoidResult.Instance;
    }
}
