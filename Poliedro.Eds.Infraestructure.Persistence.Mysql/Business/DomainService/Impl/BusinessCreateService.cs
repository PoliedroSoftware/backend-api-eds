using System.Net;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Business.Errors;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;

public class BusinessCreateService(ITenantDbContextFactory dbContextFactory) : IBusinessCreateRepository
{
    public async Task<Result<VoidResult, Error>> CreateAsync(BusinessEntity BusinessEntity)
    {
        using var context = dbContextFactory.CreateDbContext();

        var exists = await context.Business
            .AnyAsync(b => b.Name.ToLower() == BusinessEntity.Name.ToLower());

        if (exists)
        {
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
            return BusinessErrorBuilder.BusinessCreationException();

        return VoidResult.Instance;
    }
}
