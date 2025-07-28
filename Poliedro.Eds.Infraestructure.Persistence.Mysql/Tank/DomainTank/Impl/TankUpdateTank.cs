using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Tank.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Tank.DomainTank.Impl;

public class TankUpdateTank(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : ITankUpdateTank
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(TankEntity tankEntity)
    {
        if (!await EntityExists(tankEntity.IdTank))
            return TankErrorBuilder.TankNotFoundException(tankEntity.IdTank);

        using var context = dbContextFactory.CreateDbContext();
        context.Tank.Update(tankEntity);

        if (await context.SaveChangesAsync() <= 0)
            return TankErrorBuilder.TankUpdateException();
        await redisService.RemoveByPrefixAsync("tank:");

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Tank
            .AsNoTracking()
            .AnyAsync(c => c.IdTank == id);
    }
}
