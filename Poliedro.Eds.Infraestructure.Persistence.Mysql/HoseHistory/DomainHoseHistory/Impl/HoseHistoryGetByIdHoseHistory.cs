using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.HoseHistory.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;
using Poliedro.Eds.Domain.HoseHistory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.HoseHistory.DomainHoseHistory.Impl;

public class HoseHistoryGetByIdHoseHistory(DataBaseContext context, IRedisService redisService) : IHoseHistoryGetByIdHoseHistory
{
    public async Task<Result<HoseHistoryEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"hosehistory:{id}";
        var cachedData = await redisService.GetCacheAsync<HoseHistoryEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return HoseHistoryErrorBuilder.HoseHistoryNotFoundException(id);
        var data = await context.HoseHistory
            .FirstAsync(c => c.IdHoseHistory == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        return await context.HoseHistory
            .AsNoTracking()
            .AnyAsync(c => c.IdHoseHistory == id);
    }
}