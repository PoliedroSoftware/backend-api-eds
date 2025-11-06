using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.CompartimentCapacity.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.CompartimentCapacity.DomainCompartimentCapacity.Impl;

public class CompartimentCapacityGetByIdService(ITenantDbContextFactory dbContextFactory, IRedisService redisService, ILogger<CompartimentCapacityGetByIdService> logger) : ICompartimentCapacityGetByIdService
{
    public async Task<Result<CompartimentCapacityEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"compartimentCapacity:{id}";
        var cachedData = await redisService.GetCacheAsync<CompartimentCapacityEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return CompartimentCapacityErrorBuilder.CompartimentCapacityNotFoundException(id);

        using var context = dbContextFactory.CreateDbContext();

        var data = await context.CompartimentCapacity
            .FirstAsync(c => c.IdCompartimentCapacity == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        using var context = dbContextFactory.CreateDbContext();

        return await context.CompartimentCapacity
            .AsNoTracking()
            .AnyAsync(c => c.IdCompartimentCapacity == id);
    }

}
