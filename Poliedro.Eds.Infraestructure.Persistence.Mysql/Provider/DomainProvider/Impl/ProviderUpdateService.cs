using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Provider.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;
using Microsoft.EntityFrameworkCore.Internal;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Provider.DomainProvider.Impl;

public class ProviderUpdateService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IProviderUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(ProviderEntity ProviderEntity)
    {
        if (!await EntityExists(ProviderEntity.IdProvider))
            return ProviderErrorBuilder.ProviderNotFoundException(ProviderEntity.IdProvider);

        using var context = dbContextFactory.CreateDbContext();
        context.Provider.Update(ProviderEntity);

        if (await context.SaveChangesAsync() <= 0)
            return ProviderErrorBuilder.ProviderUpdateException();
        await redisService.RemoveByPrefixAsync("provider:");
        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Provider
            .AsNoTracking()
            .AnyAsync(c => c.IdProvider == id);
    }
}