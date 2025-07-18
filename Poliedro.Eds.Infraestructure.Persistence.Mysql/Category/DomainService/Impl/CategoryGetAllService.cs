using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Category.DomainCategory.Impl;

public class CategoryGetAllService(
    ITenantDbContextFactory dbContextFactory
    ) : ICategoryGetAllService
{
    public async Task<IEnumerable<CategoryEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.Category
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return data;
    }
}