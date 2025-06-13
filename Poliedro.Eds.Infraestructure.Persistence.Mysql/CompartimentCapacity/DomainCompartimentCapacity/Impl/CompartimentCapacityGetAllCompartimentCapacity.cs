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

public class CompartimentCapacityGetAllCompartimentCapacity(DataBaseContext context,IRedisService redisService) : ICompartimentCapacityGetAllCompartimentCapacity
{

    public async Task<IEnumerable<CompartimentCapacityEntity>> GetAllAsync(PaginationParams paginationParams)
    {
         var totalRows = await context.CompartimentCapacity.CountAsync();

        string cacheKey = $"compartimentCapacity:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<CompartimentCapacityEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;
        
        var data = await context.CompartimentCapacity
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

       
        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}