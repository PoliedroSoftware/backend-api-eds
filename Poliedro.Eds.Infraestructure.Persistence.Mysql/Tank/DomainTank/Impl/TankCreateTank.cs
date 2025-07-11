using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Tank.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Tank.DomainTank.Impl;

public class TankCreateTank(DataBaseContext context, IRedisService redisService) : ITankCreateTank
{
    public async Task<Result<VoidResult, Error>> CreateAsync(TankEntity tankEntity)
    {
        await context.Tank.AddAsync(tankEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return TankErrorBuilder.TankCreationException();
        await redisService.RemoveByPrefixAsync("tank:");

        return VoidResult.Instance;
    }
}