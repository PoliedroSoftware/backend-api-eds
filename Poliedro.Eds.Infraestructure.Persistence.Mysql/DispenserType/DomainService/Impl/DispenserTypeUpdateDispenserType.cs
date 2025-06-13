using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.DispenserType.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.DispenserType.DomainDispenserType;
using Poliedro.Eds.Domain.DispenserType.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.DispenserType.DomainDispenserType.Impl;

public class DispenserTypeUpdateDispenserType(DataBaseContext context, IRedisService redisService) : IDispenserTypeUpdateDispenserType
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(DispenserTypeEntity dispenserTypeEntity)
    {
        if (!await EntityExists(dispenserTypeEntity.IdType))
            return DispenserTypeErrorBuilder.DispenserTypeNotFoundException(dispenserTypeEntity.IdType);

        context.DispenserType.Update(dispenserTypeEntity);

        if (await context.SaveChangesAsync() <= 0)
            return DispenserTypeErrorBuilder.DispenserTypeUpdateException();
        await redisService.RemoveByPrefixAsync("dispensertype:");


        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.DispenserType
            .AsNoTracking()
            .AnyAsync(c => c.IdType == id);
    }
}