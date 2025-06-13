using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.HoseHistory.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;
using Poliedro.Eds.Domain.HoseHistory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.HoseHistory.DomainHoseHistory.Impl;

public class HoseHistoryUpdateHoseHistory(DataBaseContext context, IRedisService redisService) : IHoseHistoryUpdateHoseHistory
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(HoseHistoryEntity hosehistoryEntity)
    {
        if (!await EntityExists(hosehistoryEntity.IdHoseHistory))
            return HoseHistoryErrorBuilder.HoseHistoryNotFoundException(hosehistoryEntity.IdHoseHistory);

        context.HoseHistory.Update(hosehistoryEntity);

        if (await context.SaveChangesAsync() <= 0)
            return HoseHistoryErrorBuilder.HoseHistoryUpdateException();
        await redisService.RemoveByPrefixAsync("hosehistory:");

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.HoseHistory
            .AsNoTracking()
            .AnyAsync(c => c.IdHoseHistory == id);
    }
}