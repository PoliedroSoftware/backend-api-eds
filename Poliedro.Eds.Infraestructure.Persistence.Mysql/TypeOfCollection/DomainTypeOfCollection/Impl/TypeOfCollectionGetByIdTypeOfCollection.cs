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

public class TypeOfCollectionGetByIdTypeOfCollection(DataBaseContext context,IRedisService redisService) : ITypeOfCollectionGetByIdTypeOfCollection
{
    public async Task<Result<TypeOfCollectionEntity, Error>> GetByIdAsync(int id)
    {
        string cacheKey = $"typeOfCollection:{id}";
        var cachedData = await redisService.GetCacheAsync<TypeOfCollectionEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        if (!await EntityExists(id))
            return TypeOfCollectionErrorBuilder.TypeOfCollectionNotFoundException(id);

        var data = await context.TypeOfCollection
            .FirstAsync(c => c.IdTypeOfCollection == id);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return data;
    }

    private async Task<bool> EntityExists(int id)
    {
        return await context.Tank
            .AsNoTracking()
            .AnyAsync(c => c.IdTank == id);
    }

}