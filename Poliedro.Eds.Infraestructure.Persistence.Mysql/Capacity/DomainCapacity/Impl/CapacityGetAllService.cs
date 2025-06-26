using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;
using Microsoft.EntityFrameworkCore.Internal;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Capacity.DomainCapacity.Impl;

public class CapacityGetAllService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : ICapacityGetAllService
{
    public async Task<IEnumerable<CapacityEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var totalRows = await context.Capacity.CountAsync();
        string cacheKey = $"capacity:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<CapacityEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.Capacity
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();


        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}