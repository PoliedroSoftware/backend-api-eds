using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Provider.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Provider.DomainProvider.Impl;

public class ProviderUpdateService(ITenantDbContextFactory dbContextFactory, IRedisService redisService, ILogger<ProviderUpdateService> logger) : IProviderUpdateService
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
        logger.LogInformation("Successfully updated Provider");
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
