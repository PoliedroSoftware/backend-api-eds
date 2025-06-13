using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Dispensers.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Dispensers.DomainDispensers;
using Poliedro.Eds.Domain.Dispensers.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Dispensers.DomainDispensers.Impl;

public class DispensersGetByIdDispensers(DataBaseContext context, IRedisService redisService) : IDispensersGetByIdDispensers
{
    public async Task<Result<DispensersEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"dispensers:{id}";
        var cachedData = await redisService.GetCacheAsync<DispensersEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return DispensersErrorBuilder.DispensersNotFoundException(id);

        var data = await context.Dispensers
            .FirstAsync(c => c.Id == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Dispensers
            .AsNoTracking()
            .AnyAsync(c => c.Id == id);
    }
}