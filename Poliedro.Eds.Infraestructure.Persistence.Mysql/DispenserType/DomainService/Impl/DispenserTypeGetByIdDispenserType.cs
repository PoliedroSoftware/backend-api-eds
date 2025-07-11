using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.DispenserType.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.DispenserType.DomainDispenserType;
using Poliedro.Eds.Domain.DispenserType.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.DispenserType.DomainDispenserType.Impl;

public class DispenserTypeGetByIdDispenserType(DataBaseContext context, IRedisService redisService) : IDispenserTypeGetByIdDispenserType
{
    public async Task<Result<DispenserTypeEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"dispensertype:{id}";
        var cachedData = await redisService.GetCacheAsync<DispenserTypeEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return DispenserTypeErrorBuilder.DispenserTypeNotFoundException(id);

        var data = await context.DispenserType
            .FirstAsync(c => c.IdType == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.DispenserType
            .AsNoTracking()
            .AnyAsync(c => c.IdType == id);
    }
}