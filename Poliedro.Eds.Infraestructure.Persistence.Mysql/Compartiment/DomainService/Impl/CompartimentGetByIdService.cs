using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Compartiment.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Compartiment.DomainCompartiment.Impl;

public class CompartimentGetByIdService(ITenantDbContextFactory dbContextFactory, IRedisService redisService, ILogger<CompartimentGetByIdService> logger) : ICompartimentGetByIdService
{
    public async Task<Result<CompartimentEntity, Error>> GetByIdAsync(int id)
    {

        string cacheKey = $"compartiment:{id}";
        var cachedData = await redisService.GetCacheAsync<CompartimentEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return CompartimentErrorBuilder.CompartimentNotFoundException(id);

        using var context = dbContextFactory.CreateDbContext();

        var data = await context.Compartiment
            .FirstAsync(c => c.IdCompartiment == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Compartiment
            .AsNoTracking()
            .AnyAsync(c => c.IdCompartiment == id);
    }
}
