using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Eds.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Eds.DomainEds.Impl;

public class EdsUpdateService(DataBaseContext context, IRedisService redisService) : IEdsUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(EdsEntity EdsEntity)
    {
        if (!await EntityExists(EdsEntity.IdEds))
            return EdsErrorBuilder.EdsNotFoundException(EdsEntity.IdEds);

        context.Eds.Update(EdsEntity);

        if (await context.SaveChangesAsync() <= 0)
            return EdsErrorBuilder.EdsUpdateException();
        await redisService.RemoveByPrefixAsync("eds:");
        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Eds
            .AsNoTracking()
            .AnyAsync(c => c.IdEds == id);
    }
}