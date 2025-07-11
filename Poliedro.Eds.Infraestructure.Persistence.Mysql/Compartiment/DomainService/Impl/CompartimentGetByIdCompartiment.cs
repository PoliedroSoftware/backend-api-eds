using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Compartiment.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Compartiment.DomainCompartiment.Impl;

public class CompartimentGetByIdCompartiment(DataBaseContext context, IRedisService redisService) : ICompartimentGetByIdCompartiment
{
    public async Task<Result<CompartimentEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"compartiment:{id}";
        var cachedData = await redisService.GetCacheAsync<CompartimentEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return CompartimentErrorBuilder.CompartimentNotFoundException(id);

        var data = await context.Compartiment
            .FirstAsync(c => c.IdCompartment == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Compartiment
            .AsNoTracking()
            .AnyAsync(c => c.IdCompartment == id);
    }
}