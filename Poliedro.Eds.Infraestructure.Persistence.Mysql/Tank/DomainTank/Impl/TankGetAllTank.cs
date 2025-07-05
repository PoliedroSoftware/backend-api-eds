using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Tank.DomainTank.Impl;

public class TankGetAllTank(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor
    ) : ITankGetAllTank
{
    public async Task<IEnumerable<TankEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        var totalRows = await context.Tank.CountAsync();

        string cacheKey = $"tank:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<TankEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.Tank
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
        .Take(paginationParams.PageSize)
        .ToListAsync();

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}