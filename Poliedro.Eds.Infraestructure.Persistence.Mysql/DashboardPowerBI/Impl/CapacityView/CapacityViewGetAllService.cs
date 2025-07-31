using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityView.DomainCapacityView;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityView.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.DashboardPowerBI.Impl.CapacityView;

public class CapacityViewGetAllService(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor) : ICapacityViewGetAllService
{
    public async Task<IEnumerable<CapacityViewEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var totalRows = await context.CapacityView.CountAsync();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();

        string cacheKey = $"capacityView:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<CapacityViewEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.CapacityView
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();


        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}
