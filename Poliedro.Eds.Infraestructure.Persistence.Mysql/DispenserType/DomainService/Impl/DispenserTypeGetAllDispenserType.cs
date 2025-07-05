using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DispenserType.DomainDispenserType;
using Poliedro.Eds.Domain.DispenserType.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.DispenserType.DomainDispenserType.Impl;

public class DispenserTypeGetAllDispenserType(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor
    ) : IDispenserTypeGetAllDispenserType
{
    public async Task<IEnumerable<DispenserTypeEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        var totalRows = await context.DispenserType.CountAsync();

        string cacheKey = $"dispensertype:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<DispenserTypeEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;

        var data = await context.DispenserType
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();


        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}