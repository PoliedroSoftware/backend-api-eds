using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Hose.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl;

public class HoseUpdateHose(DataBaseContext context, IRedisService redisService) : IHoseUpdateHose
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(HoseEntity hoseEntity)
    {
        if (!await EntityExists(hoseEntity.IdHose))
            return HoseErrorBuilder.HoseNotFoundException(hoseEntity.IdHose);

        var existingHose = await context.Hose.FindAsync(hoseEntity.IdHose);
        if (existingHose == null)
            return HoseErrorBuilder.HoseNotFoundException(hoseEntity.IdHose);

        existingHose.AccumulatedAmount = hoseEntity.AccumulatedAmount;
        existingHose.AccumulatedGallons = hoseEntity.AccumulatedGallons;

        context.Entry(existingHose).Property(h => h.AccumulatedAmount).IsModified = true;
        context.Entry(existingHose).Property(h => h.AccumulatedGallons).IsModified = true;

        var saveResult = await context.SaveChangesAsync();
        if (saveResult <= 0)
            return HoseErrorBuilder.HoseUpdateException();

        await redisService.RemoveByPrefixAsync("hose:");

        return VoidResult.Instance;
    }

    private async Task<bool> EntityExists(int id)
    {
        return await context.Hose
            .AsNoTracking()
            .AnyAsync(c => c.IdHose == id);
    }
}