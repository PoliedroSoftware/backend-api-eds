using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessView.DomainBusinessView;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessView.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.DashboardPowerBI.Impl.BusinessView;

public class BusinessViewGetAllService(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService, ILogger<BusinessViewGetAllService> logger,
    IHttpContextAccessor httpContextAccessor
    ) : IBusinessViewGetAllService
{
    public async Task<IEnumerable<BusinessViewEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        var totalRows = await context.BusinessView.CountAsync();

        string cacheKey = $"businessView:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<BusinessViewEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.BusinessView
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        var logData = new Dictionary<string, object>
        {
            ["Request"] = "businessview",
            ["Response"] = JsonSerializer.Serialize(data),
            ["TraceId"] = cacheKey
        };

        logger.LogInformation("Response BussinessView: {@LogData}", logData);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}
