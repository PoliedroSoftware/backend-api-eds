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

public class ProviderGetByIdService(ITenantDbContextFactory dbContextFactory, IRedisService redisService, ILogger<ProviderGetByIdService> logger) : IProviderGetByIdService

{
    public async Task<Result<ProviderEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"provider:{id}";
        var cachedData = await redisService.GetCacheAsync<ProviderEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return ProviderErrorBuilder.ProviderNotFoundException(id);

        using var context = dbContextFactory.CreateDbContext();
        var data = await context.Provider
            .FirstAsync(c => c.IdProvider == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Tank
            .AsNoTracking()
            .AnyAsync(c => c.IdTank == id);
    }
}
