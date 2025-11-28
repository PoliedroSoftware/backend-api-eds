using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.PointOfSale.Entities;
using Poliedro.Eds.Domain.PosOfSale.DomainPosOfSale;
using Poliedro.Eds.Domain.PosOfSale.Dtos;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.PosOfSale.DomainPosOfSale.Impl;

public class PosOfSaleGetAllService(
    ITenantDbContextFactory dbContextFactory
    ) : IPosOfSaleGetAllService
{
    public async Task<Result<IEnumerable<PosOfSaleEntity>, Error>> GetAllAsync(PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var data = await context.PosOfSales
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return data;
    }
}
