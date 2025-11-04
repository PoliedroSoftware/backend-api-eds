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

public class CapacityUpdateService(
    ITenantDbContextFactory dbContextFactory, 
    IRedisService redisService,
    ILogger<CapacityUpdateService> logger) : ICapacityUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(CapacityEntity CapacityEntity)
    {
        logger.LogInformation("Updating capacity: {CapacityId}", CapacityEntity.IdCapacity);
        
        if (!await EntityExists(CapacityEntity.IdCapacity))
        {
            logger.LogWarning("Capacity not found for update: {CapacityId}", CapacityEntity.IdCapacity);
            return CapacityErrorBuilder.CapacityNotFoundException(CapacityEntity.IdCapacity);
        }

        using var context = dbContextFactory.CreateDbContext();
        context.Capacity.Update(CapacityEntity);

        if (await context.SaveChangesAsync() <= 0)
        {
            logger.LogError("Failed to update capacity in database: {CapacityId}", CapacityEntity.IdCapacity);
            return CapacityErrorBuilder.CapacityUpdateException();
        }
        
        await redisService.RemoveByPrefixAsync("capacity:");
        logger.LogInformation("Successfully updated capacity: {CapacityId}", CapacityEntity.IdCapacity);
        
        return VoidResult.Instance;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Capacity
            .AsNoTracking()
            .AnyAsync(c => c.IdCapacity == id);
    }
}
