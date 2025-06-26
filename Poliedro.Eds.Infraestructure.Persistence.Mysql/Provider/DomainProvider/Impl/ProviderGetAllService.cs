using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;
using Microsoft.EntityFrameworkCore.Internal;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Provider.DomainProvider.Impl;

public class ProviderGetAllService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : IProviderGetAllService
{
    public async Task<IEnumerable<ProviderEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var totalRows = await context.Provider.CountAsync();
        string cacheKey = $"provider:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<ProviderEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.Provider
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();


        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}