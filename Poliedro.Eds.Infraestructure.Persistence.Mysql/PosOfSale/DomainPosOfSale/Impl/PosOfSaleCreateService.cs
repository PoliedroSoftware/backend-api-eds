using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.PosOfSale.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PointOfSale.Entities;
using Poliedro.Eds.Domain.PosOfSale.DomainPosOfSale;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.PosOfSale.DomainPosOfSale.Impl;

public class PosOfSaleCreateService(ITenantDbContextFactory dbContextFactory) : IPosOfSaleCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(PosOfSaleEntity posOfSaleEntity)
    {
        using var context = dbContextFactory.CreateDbContext();

        var invoices = await context.PosOfSales
            .Where(x => x.InvoiceNumber != null)
            .Select(x => x.InvoiceNumber)
            .ToListAsync();

        int next = invoices
            .Where(inv => inv.All(char.IsDigit))
            .Select(inv => int.Parse(inv))
            .DefaultIfEmpty(0)
            .Max() + 1;

        posOfSaleEntity.InvoiceNumber = next.ToString();

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            await context.PosOfSales.AddAsync(posOfSaleEntity);
            await context.SaveChangesAsync();

            foreach (var detail in posOfSaleEntity.Details)
            {
                detail.PosId = posOfSaleEntity.IdPos;

                await context.PosOfSaleDetails.AddAsync(detail);
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return VoidResult.Instance;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return PosOfSaleErrorBuilder.PosOfSaleCreationException(ex.Message);
        }
    }
}
