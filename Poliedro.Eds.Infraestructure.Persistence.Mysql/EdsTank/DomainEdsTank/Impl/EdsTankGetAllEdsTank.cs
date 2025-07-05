using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;
using Poliedro.Eds.Domain.EdsTank.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EdsTank.DomainEdsTank.Impl;

public class EdsTankGetAllEdsTank(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor
    ) : IEdsTankGetAllEdsTank
{

    public async Task<IEnumerable<EdsTankEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        var totalRows = await context.EdsTank.CountAsync();

        string cacheKey = $"edsTank:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<EdsTankEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;
        
        var data = await context.EdsTank
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

       
        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}
