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

public class TypeOfCollectionUpdateTypeOfCollection(DataBaseContext context, IRedisService redisService) : ITypeOfCollectionUpdateTypeOfCollection
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(TypeOfCollectionEntity TypeOfCollectionEntity)
    {
        if (!await EntityExists(TypeOfCollectionEntity.IdTypeOfCollection))
            return TypeOfCollectionErrorBuilder.TypeOfCollectionNotFoundException(TypeOfCollectionEntity.IdTypeOfCollection);

        context.TypeOfCollection.Update(TypeOfCollectionEntity);

        if (await context.SaveChangesAsync() <= 0)
            return TypeOfCollectionErrorBuilder.TypeOfCollectionUpdateException();
        await redisService.RemoveByPrefixAsync("typeOfCollection:");
        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.TypeOfCollection
            .AsNoTracking()
            .AnyAsync(c => c.IdTypeOfCollection == id);
    }
}
