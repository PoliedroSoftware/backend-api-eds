using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessDashboardView.DomainBusinessDashboardView;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessDashboardView.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.DashboardPowerBI.Impl.BusinessDashboardView;

public class BusinessDashboardViewGetAllService(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    ILogger<BusinessDashboardViewGetAllService> logger,
    IHttpContextAccessor httpContextAccessor
    ) : IBusinessDashboardViewGetAllService
{
    public async Task<IEnumerable<BusinessDashboardViewEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        
        string cacheKey = $"businessDashboardView:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<BusinessDashboardViewEntity>>(cacheKey);
        if (cachedData is not null)
        {
            logger.LogInformation("Cache hit for BusinessDashboardView with key: {CacheKey}", cacheKey);
            return cachedData;
        }

        var data = await context.BusinessDashboardView
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        var logData = new Dictionary<string, object>
        {
            ["Request"] = "businessDashboardView",
            ["Response"] = JsonSerializer.Serialize(data),
            ["TraceId"] = cacheKey
        };

        logger.LogInformation("Response BusinessDashboardView: {@LogData}", logData);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}
