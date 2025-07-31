using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.TypeOfCollection.Errors;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.TypeOfCollection.DomainTypeOfCollection;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.TypeOfCollection.DomainTypeOfCollection.Impl;

public class TypeOfCollectionCreateTypeOfCollection(ITenantDbContextFactory dbContextFactory) : ITypeOfCollectionCreateTypeOfCollection
{
    public async Task<Result<VoidResult, Error>> CreateAsync(TypeOfCollectionEntity TypeOfCollectionEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.TypeOfCollection.AddAsync(TypeOfCollectionEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return TypeOfCollectionErrorBuilder.TypeOfCollectionCreationException();
        return VoidResult.Instance;
    }
}
