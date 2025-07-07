using Poliedro.Eds.Application.CompartimentCapacity.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.CompartimentCapacity.DomainCompartimentCapacity.Impl;

public class CompartimentCapacityCreateService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : ICompartimentCapacityCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(CompartimentCapacityEntity CompartimentCapacityEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.CompartimentCapacity.AddAsync(CompartimentCapacityEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return CompartimentCapacityErrorBuilder.CompartimentCapacityCreationException();
        await redisService.RemoveByPrefixAsync("compartimentCapacity:");
        return VoidResult.Instance;
    }
}
