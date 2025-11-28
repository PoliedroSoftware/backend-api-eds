using Poliedro.Eds.Application.PosOfSaleDetails.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.DomainPosOfSaleDetails;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.PosOfSaleDetails.DomainPosOfSaleDetails.Impl;

public class PosOfSaleDetailsCreateService(ITenantDbContextFactory dbContextFactory) : IPosOfSaleDetailsCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(PosOfSaleDetailsEntity posOfSaleEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.PosOfSaleDetails.AddAsync(posOfSaleEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return PosOfSaleDetailsErrorBuilder.PosOfSaleDetailsCreationException();
        return VoidResult.Instance;
    }
}
