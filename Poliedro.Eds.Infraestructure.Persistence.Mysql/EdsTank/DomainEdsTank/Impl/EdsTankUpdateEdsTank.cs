using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.EdsTank.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;
using Poliedro.Eds.Domain.EdsTank.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EdsTank.DomainEdsTank.Impl;

public class EdsTankUpdateEdsTank(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IEdsTankUpdateEdsTank
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(EdsTankEntity EdsTankEntity)
    {
        if (!await EntityExists(EdsTankEntity.IdEdsTank))
            return EdsTankErrorBuilder.EdsTankNotFoundException(EdsTankEntity.IdEdsTank);

        using var context = dbContextFactory.CreateDbContext();
        context.EdsTank.Update(EdsTankEntity);

        if (await context.SaveChangesAsync() <= 0)
            return EdsTankErrorBuilder.EdsTankUpdateException();
        await redisService.RemoveByPrefixAsync("edsTank:");
        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.EdsTank
            .AsNoTracking()
            .AnyAsync(c => c.IdEdsTank == id);
    }
}
