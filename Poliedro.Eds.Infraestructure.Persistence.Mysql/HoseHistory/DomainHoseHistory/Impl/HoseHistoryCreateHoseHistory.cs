using Poliedro.Eds.Application.HoseHistory.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;
using Poliedro.Eds.Domain.HoseHistory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.HoseHistory.DomainHoseHistory.Impl;

public class HoseHistoryCreateHoseHistory(
    DataBaseContext context,
    IRedisService redisService,
    IHoseUpdateHose hoseUpdateHose) : IHoseHistoryCreateHoseHistory
{
    public async Task<Result<VoidResult, Error>> CreateAsync(HoseHistoryEntity hoseHistoryEntity)
    {
        await context.HoseHistory.AddAsync(hoseHistoryEntity);
        var saveResult = await context.SaveChangesAsync();

        if (saveResult <= 0)
            return HoseHistoryErrorBuilder.HoseHistoryCreationException();

        await UpdateHoseWithLatestAccumulated(hoseHistoryEntity);
        await ClearHoseHistoryCache();

        return VoidResult.Instance;
    }

    private async Task UpdateHoseWithLatestAccumulated(HoseHistoryEntity hoseHistoryEntity)
    {
        var hoseEntity = new HoseEntity
        {
            IdHose = hoseHistoryEntity.IdHose,
            AccumulatedAmount = hoseHistoryEntity.AccumulatedAmount,
            AccumulatedGallons = hoseHistoryEntity.AccumulatedGallons,
            IdDispensers = hoseHistoryEntity.IdDispensers
        };

        await hoseUpdateHose.UpdateAsync(hoseEntity);
    }

    private async Task ClearHoseHistoryCache()
    {
        await redisService.RemoveByPrefixAsync("hosehistory:");
    }
}