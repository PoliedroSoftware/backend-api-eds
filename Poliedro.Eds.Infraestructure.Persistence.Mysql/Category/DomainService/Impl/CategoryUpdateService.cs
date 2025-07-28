using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Category.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Category.DomainCategory.Impl;

public class CategoryUpdateService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : ICategoryUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(CategoryEntity categoryEntity)
    {
        if (!await EntityExists(categoryEntity.IdCategory))
            return CategoryErrorBuilder.CategoryNotFoundException(categoryEntity.IdCategory);

        using var context = dbContextFactory.CreateDbContext();
        context.Category.Update(categoryEntity);

        if (await context.SaveChangesAsync() <= 0)
            return CategoryErrorBuilder.CategoryUpdateException();
        await redisService.RemoveByPrefixAsync("category:");

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Category
            .AsNoTracking()
            .AnyAsync(c => c.IdCategory == id);
    }
}
