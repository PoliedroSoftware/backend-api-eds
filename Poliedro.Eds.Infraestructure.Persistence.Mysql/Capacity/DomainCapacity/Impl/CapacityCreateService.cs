using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Capacity.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Capacity.DomainCapacity.Impl;

public class CapacityCreateService(ITenantDbContextFactory dbContextFactory) : ICapacityCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(CapacityEntity CapacityEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.Capacity.AddAsync(CapacityEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return CapacityErrorBuilder.CapacityCreationException();
        return VoidResult.Instance;
    }
}
