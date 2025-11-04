using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Capacity.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Capacity.DomainCapacity.Impl;

public class CapacityGetByIdService(
    ITenantDbContextFactory dbContextFactory, 
    IRedisService redisService,
    ILogger<CapacityGetByIdService> logger) : ICapacityGetByIdService

{
    public async Task<Result<CapacityEntity, Error>> GetByIdAsync(int id)
    {
        logger.LogInformation("Getting capacity by ID: {CapacityId}", id);

        string cacheKey = $"capacity:{id}";
        var cachedData = await redisService.GetCacheAsync<CapacityEntity>(cacheKey);

        if (cachedData is not null)
        {
            logger.LogInformation("Capacity found in cache: {CapacityId}", id);
            return cachedData;
        }

        if (!await EntityExists(id))
        {
            logger.LogWarning("Capacity not found: {CapacityId}", id);
            return CapacityErrorBuilder.CapacityNotFoundException(id);
        }

        using var context = dbContextFactory.CreateDbContext();

        var data = await context.Capacity
            .FirstAsync(c => c.IdCapacity == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));
        
        logger.LogInformation("Successfully retrieved capacity: {CapacityId}", id);

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
