using Poliedro.Eds.Application.Category.Errors;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Category.DomainCategory.Impl;

public class CategoryCreateService(ITenantDbContextFactory dbContextFactory) : ICategoryCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(CategoryEntity categoryEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.Category.AddAsync(categoryEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return CategoryErrorBuilder.CategoryCreationException();
        return VoidResult.Instance;
    }
}