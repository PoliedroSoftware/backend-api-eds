using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Tank.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Tank.DomainTank.Impl;

public class TankGetByIdTank(DataBaseContext context, IRedisService redisService) : ITankGetByIdTank
{
    public async Task<Result<TankEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"tank:{id}";
        var cachedData = await redisService.GetCacheAsync<TankEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return TankErrorBuilder.TankNotFoundException(id);

        var data = await context.Tank
            .FirstAsync(c => c.IdTank == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        return await context.Tank
            .AsNoTracking()
            .AnyAsync(c => c.IdTank == id);
    }
}