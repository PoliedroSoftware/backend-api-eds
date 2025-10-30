using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Business.Errors;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;

public class BusinessCreateService(
    ITenantDbContextFactory dbContextFactory,
    ILogger<BusinessCreateService> logger) : IBusinessCreateRepository
{
    public async Task<Result<VoidResult, Error>> CreateAsync(BusinessEntity BusinessEntity)
    {
        logger.LogInformation("Creating new business with name: {BusinessName}", BusinessEntity.Name);
        
        using var context = dbContextFactory.CreateDbContext();

        var exists = await context.Business
            .AnyAsync(b => b.Name.ToLower() == BusinessEntity.Name.ToLower());

        if (exists)
        {
            logger.LogWarning("Business creation failed - Name already exists: {BusinessName}", BusinessEntity.Name);
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance(
                    code: "BusinessAlreadyExists",
                    description: "The business name already exists. Please enter a different name.",
                    httpStatusCode: HttpStatusCode.Conflict
                )
            );
        }

        await context.Business.AddAsync(BusinessEntity);
        var saved = await context.SaveChangesAsync() > 0;
        if (!saved)
        {
            logger.LogError("Failed to save business to database: {BusinessName}", BusinessEntity.Name);
            return BusinessErrorBuilder.BusinessCreationException();
        }

        logger.LogInformation("Successfully created business: {BusinessName}", BusinessEntity.Name);
        return VoidResult.Instance;
    }
}
