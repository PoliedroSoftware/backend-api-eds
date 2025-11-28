using System;
using System.Collections.Generic;
using System.Text;
using Poliedro.Eds.Application.Hose.Errors;
using Poliedro.Eds.Application.PosOfSale.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Domain.Phone.Entities;
using Poliedro.Eds.Domain.PointOfSale.Entities;
using Poliedro.Eds.Domain.PosOfSale.DomainPosOfSale;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.PosOfSale.DomainPosOfSale.Impl;

public class PosOfSaleCreateService(ITenantDbContextFactory dbContextFactory) : IPosOfSaleCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(PosOfSaleEntity posOfSaleEntity)
    {
        using var context = dbContextFactory.CreateDbContext();
        await context.PosOfSales.AddAsync(posOfSaleEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return PosOfSaleErrorBuilder.PosOfSaleCreationException();
        return VoidResult.Instance;
    }
}
