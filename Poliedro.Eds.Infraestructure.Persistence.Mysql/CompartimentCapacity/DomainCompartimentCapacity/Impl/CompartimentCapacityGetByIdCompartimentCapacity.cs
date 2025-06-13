using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.CompartimentCapacity.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.CompartimentCapacity.DomainCompartimentCapacity.Impl;

public class CompartimentCapacityGetByIdCompartimentCapacity(DataBaseContext context,IRedisService redisService) : ICompartimentCapacityGetByIdCompartimentCapacity
{
    public async Task<Result<CompartimentCapacityEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"compartimentCapacity:{id}";
        var cachedData = await redisService.GetCacheAsync<CompartimentCapacityEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return CompartimentCapacityErrorBuilder.CompartimentCapacityNotFoundException(id);

        var data = await context.CompartimentCapacity
            .FirstAsync(c => c.IdCompartimentCapacity == id);

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