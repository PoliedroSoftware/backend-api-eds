

using Poliedro.Eds.Application.DispenserType.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.DispenserType.DomainDispenserType;
using Poliedro.Eds.Domain.DispenserType.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.DispenserType.DomainDispenserType.Impl;

public class DispenserTypeCreateDispenserType(DataBaseContext context, IRedisService redisService) : IDispenserTypeCreateDispenserType
{
    public async Task<Result<VoidResult, Error>> CreateAsync(DispenserTypeEntity dispenserTypeEntity)
    {
        await context.DispenserType.AddAsync(dispenserTypeEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return DispenserTypeErrorBuilder.DispenserTypeCreationException();
        await redisService.RemoveByPrefixAsync("dispensertype:");

        return VoidResult.Instance;
    }
}