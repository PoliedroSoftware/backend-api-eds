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

public class EdsUpdateService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IEdsUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(EdsEntity EdsEntity)
    {
        if (!await EntityExists(EdsEntity.IdEds))
            return EdsErrorBuilder.EdsNotFoundException(EdsEntity.IdEds);

        using var context = dbContextFactory.CreateDbContext();
        context.Eds.Update(EdsEntity);

        if (await context.SaveChangesAsync() <= 0)
            return EdsErrorBuilder.EdsUpdateException();
        await redisService.RemoveByPrefixAsync("eds:");
        return VoidResult.Instance;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Eds
            .AsNoTracking()
            .AnyAsync(c => c.IdEds == id);
    }
}
