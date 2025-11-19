using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityDashboardView.DomainCapacityDashboardView;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityDashboardView.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.DashboardPowerBI.Impl.CapacityDashboardView;

public class CapacityDashboardViewGetAllService(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    ILogger<CapacityDashboardViewGetAllService> logger,
    IHttpContextAccessor httpContextAccessor
    ) : ICapacityDashboardViewGetAllService
{
    public async Task<IEnumerable<CapacityDashboardViewEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        
        string cacheKey = $"capacityDashboardView:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<CapacityDashboardViewEntity>>(cacheKey);
        if (cachedData is not null)
        {
            logger.LogInformation("Cache hit for CapacityDashboardView with key: {CacheKey}", cacheKey);
            return cachedData;
        }

        var data = await context.CapacityDashboardView
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        var logData = new Dictionary<string, object>
        {
            ["Request"] = "capacityDashboardView",
            ["Response"] = JsonSerializer.Serialize(data),
            ["TraceId"] = cacheKey
        };

        logger.LogInformation("Response CapacityDashboardView: {@LogData}", logData);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}
