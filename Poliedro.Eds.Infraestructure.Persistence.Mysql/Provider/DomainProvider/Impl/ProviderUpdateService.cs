using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Provider.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Provider.DomainProvider.Impl;

public class ProviderUpdateService(DataBaseContext context, IRedisService redisService) : IProviderUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(ProviderEntity ProviderEntity)
    {
        if (!await EntityExists(ProviderEntity.IdProvider))
            return ProviderErrorBuilder.ProviderNotFoundException(ProviderEntity.IdProvider);

        context.Provider.Update(ProviderEntity);

        if (await context.SaveChangesAsync() <= 0)
            return ProviderErrorBuilder.ProviderUpdateException();
        await redisService.RemoveByPrefixAsync("provider:");
        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Provider
            .AsNoTracking()
            .AnyAsync(c => c.IdProvider == id);
    }
}