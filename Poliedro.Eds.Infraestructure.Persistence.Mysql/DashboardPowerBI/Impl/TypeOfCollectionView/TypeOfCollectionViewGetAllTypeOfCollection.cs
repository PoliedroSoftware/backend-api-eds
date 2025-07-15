using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.TypeOfCollectionView.DomainTypeOfCollectionView;
using Poliedro.Eds.Domain.DashboardPowerBI.TypeOfCollectionView.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.DashboardPowerBI.Impl.TypeOfCollectionView;

public class TypeOfCollectionViewGetAllTypeOfCollection(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor
    ) : ITypeOfCollectionViewGetAllTypeOfCollection
{

    public async Task<IEnumerable<TypeOfCollectionViewEntity>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        var totalRows = await context.TypeOfCollectionView.CountAsync();

        string cacheKey = $"typeOfCollectionView:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<TypeOfCollectionViewEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;
        
        var data = await context.TypeOfCollectionView
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

       
        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}