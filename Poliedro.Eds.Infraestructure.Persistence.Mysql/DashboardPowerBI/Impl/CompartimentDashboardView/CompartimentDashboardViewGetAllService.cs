using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.CompartimentDashboardView.DomainCompartimentDashboardView;
using Poliedro.Eds.Domain.DashboardPowerBI.CompartimentDashboardView.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.DashboardPowerBI.Impl.CompartimentDashboardView;

public class CompartimentDashboardViewGetAllService(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    ILogger<CompartimentDashboardViewGetAllService> logger,
    IHttpContextAccessor httpContextAccessor
    ) : ICompartimentDashboardViewGetAllService
{
    public async Task<IEnumerable<CompartimentDashboardViewEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        
        string cacheKey = $"compartimentDashboardView:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<CompartimentDashboardViewEntity>>(cacheKey);
        if (cachedData is not null)
        {
            logger.LogInformation("Cache hit for CompartimentDashboardView with key: {CacheKey}", cacheKey);
            return cachedData;
        }

        var data = await context.CompartimentDashboardView
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        var logData = new Dictionary<string, object>
        {
            ["Request"] = "compartimentDashboardView",
            ["Response"] = JsonSerializer.Serialize(data),
            ["TraceId"] = cacheKey
        };

        logger.LogInformation("Response CompartimentDashboardView: {@LogData}", logData);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}
