using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Eds.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Eds.DomainEds.Impl;

public class EdsGetByIdService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IEdsGetByIdService

{
    public async Task<Result<EdsEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"eds:{id}";
        var cachedData = await redisService.GetCacheAsync<EdsEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return EdsErrorBuilder.EdsNotFoundException(id);

        using var context = dbContextFactory.CreateDbContext();
        var data = await context.Eds
            .FirstAsync(c => c.IdEds == id);

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
