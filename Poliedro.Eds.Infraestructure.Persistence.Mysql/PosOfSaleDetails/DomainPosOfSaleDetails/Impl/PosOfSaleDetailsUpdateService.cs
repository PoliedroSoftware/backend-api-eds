using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.PosOfSaleDetails.Errors;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.DomainPosOfSaleDetails;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.PosOfSaleDetails.DomainPosOfSaleDetails.Impl;

public class PosOfSaleDetailsUpdateService(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IDomainEventDispatcher domainEventDispatcher,
    IHttpContextAccessor httpContextAccessor) : IPosOfSaleDetailsUpdateService
{

    public async Task<Result<VoidResult, Error>> UpdateAsync(PosOfSaleDetailsEntity posOfSaleEntity)
    {
        if (!await EntityExists(posOfSaleEntity.IdDetail))
            return PosOfSaleDetailsErrorBuilder.PosOfSaleDetailsNotFoundException(posOfSaleEntity.IdDetail);

        using var context = dbContextFactory.CreateDbContext();
        context.PosOfSaleDetails.Update(posOfSaleEntity);

        if (await context.SaveChangesAsync() <= 0)
            return PosOfSaleDetailsErrorBuilder.PosOfSaleDetailsUpdateException();

        var result = Result<VoidResult, Error>.Success(VoidResult.Instance);

        await RedisHelper.InvalidateDistributedCacheAsync(
            result,
            redisService,
            domainEventDispatcher,
            httpContextAccessor,
            "posOfSaleDetails",
            "update",
            posOfSaleEntity.IdDetail);

        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.POS_OF_SALE_DETAILS);

        return VoidResult.Instance;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.PosOfSaleDetails
            .AsNoTracking()
            .AnyAsync(c => c.IdDetail == id);
    }
}

