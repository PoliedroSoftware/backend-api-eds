using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Eds.Errors;
using Poliedro.Eds.Application.PosOfSale.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.PointOfSale.Entities;
using Poliedro.Eds.Domain.PosOfSale.DomainPosOfSale;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.PosOfSale.DomainPosOfSale.Impl;

public class PosOfSaleGetByIdService(ITenantDbContextFactory dbContextFactory) : IPosOfSaleGetByIdService
{
    public async Task<Result<PosOfSaleEntity, Error>> GetByIdAsync(int id)
    {
        using var context = dbContextFactory.CreateDbContext();

        var data = await context.PosOfSales
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdPos == id);

        if (data is null)
            return PosOfSaleErrorBuilder.PosOfSaleNotFoundException(id);

        return data;
    }
}
