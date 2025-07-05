using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Ports.Redis;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Http;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Eds.DomainEds.Impl;

public class EdsGetAllService(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor
    ) : IEdsGetAllService
{
    public async Task<IEnumerable<EdsEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        var totalRows = await context.Eds.CountAsync();
        string cacheKey = $"eds:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<EdsEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.Eds
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();


        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}