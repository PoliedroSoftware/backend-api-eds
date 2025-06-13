using Poliedro.Eds.Application.Dispensers.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Dispensers.DomainDispensers;
using Poliedro.Eds.Domain.Dispensers.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Dispensers.DomainDispensers.Impl;

public class DispensersCreateDispensers(DataBaseContext context, IRedisService redisService) : IDispensersCreateDispensers
{
    public async Task<Result<VoidResult, Error>> CreateAsync(DispensersEntity dispensersEntity)
    {
        await context.Dispensers.AddAsync(dispensersEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return DispensersErrorBuilder.DispensersCreationException();
        await redisService.RemoveByPrefixAsync("dispensers:");

        return VoidResult.Instance;
    }
}