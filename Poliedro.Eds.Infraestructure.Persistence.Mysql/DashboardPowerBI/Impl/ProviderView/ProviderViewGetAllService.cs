using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.ProviderView.DomainProviderView;
using Poliedro.Eds.Domain.DashboardPowerBI.ProviderView.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.DashboardPowerBI.Impl.ProviderView;

public class ProviderViewGetAllService(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor
    ) : IProviderViewGetAllService
{
    public async Task<IEnumerable<ProviderViewEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var totalRows = await context.ProviderView.CountAsync();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();

        string cacheKey = $"providerView:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<ProviderViewEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.ProviderView
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();


        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}
