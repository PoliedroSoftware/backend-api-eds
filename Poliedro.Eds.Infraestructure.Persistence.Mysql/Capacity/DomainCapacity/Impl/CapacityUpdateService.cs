using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Capacity.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Capacity.DomainCapacity.Impl;

public class CapacityUpdateService(DataBaseContext context, IRedisService redisService) : ICapacityUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(CapacityEntity CapacityEntity)
    {
        if (!await EntityExists(CapacityEntity.IdCapacity))
            return CapacityErrorBuilder.CapacityNotFoundException(CapacityEntity.IdCapacity);

        context.Capacity.Update(CapacityEntity);

        if (await context.SaveChangesAsync() <= 0)
            return CapacityErrorBuilder.CapacityUpdateException();
        await redisService.RemoveByPrefixAsync("capacity:");
        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Capacity
            .AsNoTracking()
            .AnyAsync(c => c.IdCapacity == id);
    }
}