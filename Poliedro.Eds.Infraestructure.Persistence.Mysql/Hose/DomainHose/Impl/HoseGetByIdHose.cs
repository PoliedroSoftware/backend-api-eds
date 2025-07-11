using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Hose.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl;

public class HoseGetByIdHose(DataBaseContext context,IRedisService redisService) : IHoseGetByIdHose
{
    public async Task<Result<HoseEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"hose:{id}";
        var cachedData = await redisService.GetCacheAsync<HoseEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return HoseErrorBuilder.HoseNotFoundException(id);
        var data = await context.Hose
            .FirstAsync(c => c.IdHose == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        return await context.Hose
            .AsNoTracking()
            .AnyAsync(c => c.IdHose == id);
    }
}