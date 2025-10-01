using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Hose.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl;

public class HoseUpdateHose(
    ITenantDbContextFactory dbContextFactory, 
    IRedisService redisService,
    IDomainEventDispatcher domainEventDispatcher,
    IHttpContextAccessor httpContextAccessor) : IHoseUpdateHose
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(HoseEntity hoseEntity)
    {
        if (!await EntityExists(hoseEntity.IdHose))
            return HoseErrorBuilder.HoseNotFoundException(hoseEntity.IdHose);

        using var context = dbContextFactory.CreateDbContext();
        var existingHose = await context.Hose.FindAsync(hoseEntity.IdHose);
        if (existingHose == null)
            return HoseErrorBuilder.HoseNotFoundException(hoseEntity.IdHose);

        existingHose.AccumulatedAmount = hoseEntity.AccumulatedAmount;
        existingHose.AccumulatedGallons = hoseEntity.AccumulatedGallons;

        context.Entry(existingHose).Property(h => h.AccumulatedAmount).IsModified = true;
        context.Entry(existingHose).Property(h => h.AccumulatedGallons).IsModified = true;

        var saveResult = await context.SaveChangesAsync();
        if (saveResult <= 0)
            return HoseErrorBuilder.HoseUpdateException();

        var result = Result<VoidResult, Error>.Success(VoidResult.Instance);
        
        // Invalidar cache usando sistema distribuido
        await RedisHelper.InvalidateDistributedCacheAsync(
            result,
            redisService,
            domainEventDispatcher,
            httpContextAccessor,
            "hose",
            "update",
            hoseEntity.IdHose);

        // También invalidar usando el método tradicional como respaldo
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, 
            KeyRedisConstants.HOSE,
            KeyRedisConstants.HOSE_HISTORY,
            "LastAccumulated:");

        return VoidResult.Instance;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Hose
            .AsNoTracking()
            .AnyAsync(c => c.IdHose == id);
    }
}
