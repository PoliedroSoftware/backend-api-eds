using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Eds.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Eds.DomainEds.Impl;

public class EdsCreateService(ITenantDbContextFactory dbContextFactory, ILogger<EdsCreateService> logger) : IEdsCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(EdsEntity EdsEntity)
    {
        logger.LogInformation("Creating new EDS: {EdsName}", EdsEntity.Name);
        
        using var context = dbContextFactory.CreateDbContext();
        await context.Eds.AddAsync(EdsEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
        {
            logger.LogError("Failed to save EDS to database: {EdsName}", EdsEntity.Name);
            return EdsErrorBuilder.EdsCreationException();
        }
        
        logger.LogInformation("Successfully created EDS: {EdsName}", EdsEntity.Name);
        return VoidResult.Instance;
    }
}
