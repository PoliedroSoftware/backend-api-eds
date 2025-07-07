using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Islander.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Islander.Domainislander.Impl;

public class IslanderGetByIdIslander(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IIslanderGetByIdIslander
{
    public async Task<Result<IslanderEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"islander:{id}";
        var cachedData = await redisService.GetCacheAsync<IslanderEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        using var context = dbContextFactory.CreateDbContext();

        if (!await EntityExists(id))
            return IslanderErrorBuilder.IslanderNotFoundException(id);
        var data = await context.Islander
            .FirstAsync(c => c.IdIslander == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();

        return await context.Islander
            .AsNoTracking()
            .AnyAsync(c => c.IdIslander == id);
    }
}