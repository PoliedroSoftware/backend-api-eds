using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Capacity.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;
using Microsoft.EntityFrameworkCore.Internal;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Capacity.DomainCapacity.Impl;

public class CapacityGetByIdService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : ICapacityGetByIdService

{
    public async Task<Result<CapacityEntity, Error>> GetByIdAsync(int id)
    {

        string cacheKey = $"capacity:{id}";
        var cachedData = await redisService.GetCacheAsync<CapacityEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return CapacityErrorBuilder.CapacityNotFoundException(id);

        using var context = dbContextFactory.CreateDbContext();

        var data = await context.Capacity
            .FirstAsync(c => c.IdCapacity == id);

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