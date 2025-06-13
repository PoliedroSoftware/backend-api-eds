using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Dispensers.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Dispensers.DomainDispensers;
using Poliedro.Eds.Domain.Dispensers.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Dispensers.DomainDispensers.Impl;

public class DispensersUpdateDispensers(DataBaseContext context, IRedisService redisService) : IDispensersUpdateDispensers
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(DispensersEntity dispensersEntity)
    {
        if (!await EntityExists(dispensersEntity.Id))
            return DispensersErrorBuilder.DispensersNotFoundException(dispensersEntity.Id);

        context.Dispensers.Update(dispensersEntity);

        if (await context.SaveChangesAsync() <= 0)
            return DispensersErrorBuilder.DispensersUpdateException();
        await redisService.RemoveByPrefixAsync("dispensers:");

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Dispensers
            .AsNoTracking()
            .AnyAsync(c => c.Id == id);
    }
}