using Poliedro.Eds.Application.Capacity.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Capacity.DomainCapacity.Impl;

public class CapacityCreateService(DataBaseContext context, IRedisService redisService) : ICapacityCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(CapacityEntity CapacityEntity)
    {
        await context.Capacity.AddAsync(CapacityEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return CapacityErrorBuilder.CapacityCreationException();
        await redisService.RemoveByPrefixAsync("capacity:");
        return VoidResult.Instance;
    }
}