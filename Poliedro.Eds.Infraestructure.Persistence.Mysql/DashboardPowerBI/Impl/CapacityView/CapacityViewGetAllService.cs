using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityView.DomainCapacityView;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityView.Entities;

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