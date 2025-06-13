using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Category.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Category.DomainCategory;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Category.DomainCategory.Impl;

public class CategoryUpdateCategory(DataBaseContext context, IRedisService redisService) : ICategoryUpdateCategory
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(CategoryEntity categoryEntity)
    {
        if (!await EntityExists(categoryEntity.IdCategory))
            return CategoryErrorBuilder.CategoryNotFoundException(categoryEntity.IdCategory);

        context.Category.Update(categoryEntity);

        if (await context.SaveChangesAsync() <= 0)
            return CategoryErrorBuilder.CategoryUpdateException();
        await redisService.RemoveByPrefixAsync("category:");

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Category
            .AsNoTracking()
            .AnyAsync(c => c.IdCategory == id);
    }
}