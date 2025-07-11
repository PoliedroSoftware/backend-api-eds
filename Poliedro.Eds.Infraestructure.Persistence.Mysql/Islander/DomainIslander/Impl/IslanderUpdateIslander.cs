using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Islander.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Islander.DomainIslander.Impl;

public class IslanderUpdateIslander(DataBaseContext context, IRedisService redisService) : IIslanderUpdateIslander
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(IslanderEntity islanderEntity)
    {
        if (!await EntityExists(islanderEntity.IdIslander))
            return IslanderErrorBuilder.IslanderNotFoundException(islanderEntity.IdIslander);

        context.Islander.Update(islanderEntity);

        if (await context.SaveChangesAsync() <= 0)
            return IslanderErrorBuilder.IslanderUpdateException();
        await redisService.RemoveByPrefixAsync("islander:");

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Islander
            .AsNoTracking()
            .AnyAsync(c => c.IdIslander == id);
    }
}