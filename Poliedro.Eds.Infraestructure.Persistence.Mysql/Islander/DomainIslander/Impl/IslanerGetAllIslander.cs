using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Islander.Domainislander.Impl;

public class IslanderGetAllIslander(DataBaseContext context,IRedisService redisService) : IIslanderGetAllIslander
{
    public async Task<IEnumerable<IslanderEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        var totalRows = await context.Islander.CountAsync();

        string cacheKey = $"islander:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<IslanderEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.Islander
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}