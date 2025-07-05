using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;
using Poliedro.Eds.Domain.HoseHistory.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.HoseHistory.DomainHoseHistory.Impl;

public class HoseHistoryGetAllHoseHistory(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor
    ) : IHoseHistoryGetAllHoseHistory
{
    public async Task<IEnumerable<HoseHistoryEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        var totalRows = await context.HoseHistory.CountAsync();

        string cacheKey = $"hosehistory:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<HoseHistoryEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.HoseHistory
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
        .Take(paginationParams.PageSize)
        .ToListAsync();

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}