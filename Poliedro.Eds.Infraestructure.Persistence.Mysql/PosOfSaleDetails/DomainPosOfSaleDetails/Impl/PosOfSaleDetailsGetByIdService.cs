using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.PosOfSaleDetails.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.DomainPosOfSaleDetails;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.PosOfSaleDetails.DomainPosOfSaleDetails.Impl;

public class PosOfSaleDetailsGetByIdService(ITenantDbContextFactory dbContextFactory) : IPosOfSaleDetailsGetByIdService
{
    public async Task<Result<PosOfSaleDetailsEntity, Error>> GetByIdAsync(int id)
    {
        using var context = dbContextFactory.CreateDbContext();

        var data = await context.PosOfSaleDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdDetail == id);

        if (data is null)
            return PosOfSaleDetailsErrorBuilder.PosOfSaleDetailsNotFoundException(id);

        return data;
    }
}
