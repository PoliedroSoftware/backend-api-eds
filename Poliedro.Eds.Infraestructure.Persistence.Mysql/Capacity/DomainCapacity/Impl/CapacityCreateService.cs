using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Capacity.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Capacity.DomainCapacity.Impl;

public class CapacityCreateService(
    ITenantDbContextFactory dbContextFactory,
    ILogger<CapacityCreateService> logger) : ICapacityCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(CapacityEntity CapacityEntity)
    {
        logger.LogInformation("Creating new capacity with code: {CapacityCode}", CapacityEntity.Code);
        
        using var context = dbContextFactory.CreateDbContext();
        await context.Capacity.AddAsync(CapacityEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
        {
            logger.LogError("Failed to save capacity to database: {CapacityCode}", CapacityEntity.Code);
            return CapacityErrorBuilder.CapacityCreationException();
        }
        
        logger.LogInformation("Successfully created capacity: {CapacityCode}", CapacityEntity.Code);
        return VoidResult.Instance;
    }
}
