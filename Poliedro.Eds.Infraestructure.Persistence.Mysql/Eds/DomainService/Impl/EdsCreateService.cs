using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
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

public class EdsCreateService(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IDomainEventDispatcher domainEventDispatcher,
    IHttpContextAccessor httpContextAccessor) : IEdsCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(EdsEntity edsEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.Eds.AddAsync(edsEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return EdsErrorBuilder.EdsCreationException();

        var successResult = Result<VoidResult, Error>.Success(VoidResult.Instance);

        // Invalidar cache usando sistema distribuido
        await RedisHelper.InvalidateDistributedCacheAsync(
            successResult,
            redisService,
            domainEventDispatcher,
            httpContextAccessor,
            "eds",
            "create",
            edsEntity.IdEds);

        // También invalidar usando el método tradicional como respaldo
        await RedisHelper.RemoveCacheIfSuccessAsync(successResult, redisService, KeyRedisConstants.EDS);

        return VoidResult.Instance;
    }
}
