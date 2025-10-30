using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Category.DomainCategory.Impl;

public class CategoryGetAllService(
    ITenantDbContextFactory dbContextFactory,
    ILogger<CategoryGetAllService> logger
    ) : ICategoryGetAllService
{
    public async Task<IEnumerable<CategoryEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        logger.LogInformation("Getting all categories with pagination - Page: {PageNumber}, PageSize: {PageSize}", 
            paginationParams.PageNumber, paginationParams.PageSize);
        
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.Category
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        
        logger.LogInformation("Successfully retrieved {Count} categories", data.Count());
        
        return data;
    }
}
