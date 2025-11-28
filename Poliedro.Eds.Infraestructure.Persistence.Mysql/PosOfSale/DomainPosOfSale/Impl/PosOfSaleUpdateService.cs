using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.PosOfSale.Errors;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PointOfSale.Entities;
using Poliedro.Eds.Domain.PosOfSale.DomainPosOfSale;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.PosOfSale.DomainPosOfSale.Impl;

public class PosOfSaleUpdateService(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IDomainEventDispatcher domainEventDispatcher,
    IHttpContextAccessor httpContextAccessor) : IPosOfSaleUpdateService
{

    public async Task<Result<VoidResult, Error>> UpdateAsync(PosOfSaleEntity posOfSaleEntity)
    {
        if (!await EntityExists(posOfSaleEntity.IdPos))
            return PosOfSaleErrorBuilder.PosOfSaleNotFoundException(posOfSaleEntity.IdPos);

        using var context = dbContextFactory.CreateDbContext();
        context.PosOfSales.Update(posOfSaleEntity);

        if (await context.SaveChangesAsync() <= 0)
            return PosOfSaleErrorBuilder.PosOfSaleUpdateException();

        var result = Result<VoidResult, Error>.Success(VoidResult.Instance);

        await RedisHelper.InvalidateDistributedCacheAsync(
            result,
            redisService,
            domainEventDispatcher,
            httpContextAccessor,
            "posOfSale",
            "update",
            posOfSaleEntity.IdPos);

        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.POS_OF_SALE);

        return VoidResult.Instance;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.PosOfSales
            .AsNoTracking()
            .AnyAsync(c => c.IdPos == id);
    }
}
