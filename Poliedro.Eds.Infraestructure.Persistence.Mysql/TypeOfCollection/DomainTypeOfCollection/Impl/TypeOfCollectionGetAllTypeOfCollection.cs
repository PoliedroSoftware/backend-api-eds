using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.TypeOfCollection.Errors;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.TypeOfCollection.DomainTypeOfCollection;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.TypeOfCollection.DomainTypeOfCollection.Impl;

public class TypeOfCollectionGetAllTypeOfCollection(DataBaseContext context,IRedisService redisService) : ITypeOfCollectionGetAllTypeOfCollection
{

    public async Task<IEnumerable<TypeOfCollectionEntity>> GetAllAsync(PaginationParams paginationParams)
    {
         var totalRows = await context.TypeOfCollection.CountAsync();

        string cacheKey = $"typeOfCollection:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<TypeOfCollectionEntity>>(cacheKey);
        if (cachedData is not null) return cachedData;
        
        var data = await context.TypeOfCollection
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

       
        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }
}