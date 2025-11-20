using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Eds.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Eds.DomainEds.Impl;

public class EdsUpdateService(
    ITenantDbContextFactory dbContextFactory, 
    IRedisService redisService,
    IDomainEventDispatcher domainEventDispatcher,
    IHttpContextAccessor httpContextAccessor) : IEdsUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(EdsEntity edsEntity)
    {
        if (!await EntityExists(edsEntity.IdEds))
            return EdsErrorBuilder.EdsNotFoundException(edsEntity.IdEds);

        using var context = dbContextFactory.CreateDbContext();
        context.Eds.Update(edsEntity);

        if (await context.SaveChangesAsync() <= 0)
            return EdsErrorBuilder.EdsUpdateException();

        var result = Result<VoidResult, Error>.Success(VoidResult.Instance);
        
        // Invalidar cache usando sistema distribuido
        await RedisHelper.InvalidateDistributedCacheAsync(
            result,
            redisService,
            domainEventDispatcher,
            httpContextAccessor,
            "eds",
            "update",
            edsEntity.IdEds);

        // También invalidar usando el método tradicional como respaldo
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.EDS);

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
