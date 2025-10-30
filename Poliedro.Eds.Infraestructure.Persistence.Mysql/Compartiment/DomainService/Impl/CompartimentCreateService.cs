using Poliedro.Eds.Application.Compartiment.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Compartiment.DomainCompartiment.Impl;

public class CompartimentCreateService(ITenantDbContextFactory dbContextFactory, ILogger<CompartimentCreateService> logger) : ICompartimentCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(CompartimentEntity compartimentEntity)
    {
        logger.LogInformation("Creating new compartiment for tank: {TankId}", compartimentEntity.IdTank);
        
        using var context = dbContextFactory.CreateDbContext();
        await context.Compartiment.AddAsync(compartimentEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
        {
            logger.LogError("Failed to save compartiment to database for tank: {TankId}", compartimentEntity.IdTank);
            return CompartimentErrorBuilder.CompartimentCreationException();
        }
        
        logger.LogInformation("Successfully created compartiment for tank: {TankId}", compartimentEntity.IdTank);
        logger.LogInformation("Successfully created Compartiment");
return VoidResult.Instance;
    }
}
