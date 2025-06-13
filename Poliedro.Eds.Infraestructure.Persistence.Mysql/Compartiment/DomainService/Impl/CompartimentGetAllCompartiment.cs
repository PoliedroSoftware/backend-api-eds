using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Compartiment.DomainCompartiment.Impl;

public class CompartimentGetAllCompartiment(DataBaseContext context, IRedisService redisService) : ICompartimentGetAllCompartiment
{
    public async Task<IEnumerable<CompartimentEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        var totalRows = await context.Compartiment.CountAsync();
        string cacheKey = $"compartiment:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<CompartimentEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.Compartiment
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();


        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}