using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Phone.DomainServices.Create;
using Poliedro.Eds.Domain.Phone.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Application.Phone.Errors;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Phone.DomainService.Impl;

public class PhoneCreateService(
    ITenantDbContextFactory dbContextFactory,
    ILogger<PhoneCreateService> logger) : IPhoneCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(PhoneEntity phoneEntity)
    {
        logger.LogInformation("Creating new phone with number: {PhoneNumber}", phoneEntity.Number);
        
        using var context = dbContextFactory.CreateDbContext();
        await context.Phone.AddAsync(phoneEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
        {
            logger.LogError("Failed to save phone to database: {PhoneNumber}", phoneEntity.Number);
            return PhoneErrorBuilder.PhoneCreationException();
        }
        
        logger.LogInformation("Successfully created phone: {PhoneNumber}", phoneEntity.Number);
        return VoidResult.Instance;
    }
}
