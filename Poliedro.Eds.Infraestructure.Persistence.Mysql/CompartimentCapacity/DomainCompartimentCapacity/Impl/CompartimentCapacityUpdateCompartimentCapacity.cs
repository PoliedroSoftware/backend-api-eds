using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.CompartimentCapacity.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.CompartimentCapacity.DomainCompartimentCapacity.Impl;

public class CompartimentCapacityUpdateCompartimentCapacity(DataBaseContext context, IRedisService redisService) : ICompartimentCapacityUpdateCompartimentCapacity
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(CompartimentCapacityEntity CompartimentCapacityEntity)
    {
        if (!await EntityExists(CompartimentCapacityEntity.IdCompartimentCapacity))
            return CompartimentCapacityErrorBuilder.CompartimentCapacityNotFoundException(CompartimentCapacityEntity.IdCompartimentCapacity);

        context.CompartimentCapacity.Update(CompartimentCapacityEntity);

        if (await context.SaveChangesAsync() <= 0)
            return CompartimentCapacityErrorBuilder.CompartimentCapacityUpdateException();
        await redisService.RemoveByPrefixAsync("comparimentCapacity:");
        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.CompartimentCapacity
            .AsNoTracking()
            .AnyAsync(c => c.IdCompartimentCapacity == id);
    }
}
