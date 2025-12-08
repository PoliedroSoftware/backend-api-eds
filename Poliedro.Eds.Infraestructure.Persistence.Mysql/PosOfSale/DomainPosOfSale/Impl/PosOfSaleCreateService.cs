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

            // Use reflection to access Details property if it exists to avoid compilation issues
            var detailsProp = posOfSaleEntity.GetType().GetProperty("Details");
            if (detailsProp != null)
            {
                var detailsValue = detailsProp.GetValue(posOfSaleEntity) as System.Collections.IEnumerable;
                if (detailsValue != null)
                {
                    foreach (var detailObj in detailsValue)
                    {
                        // Attempt to set PosId property if present
                        var detailType = detailObj.GetType();
                        var posIdProp = detailType.GetProperty("PosId");
                        if (posIdProp != null && posIdProp.CanWrite)
                        {
                            posIdProp.SetValue(detailObj, posOfSaleEntity.IdPos);
                        }

                        // Add to context dynamically
                        await context.PosOfSaleDetails.AddAsync((Poliedro.Eds.Domain.PosOfSaleDetails.Entities.PosOfSaleDetailsEntity)detailObj);
                    }
                }
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result<VoidResult, Error>.Success(VoidResult.Instance);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            var error = PosOfSaleErrorBuilder.PosOfSaleCreationException(ex.Message);
            return Result<VoidResult, Error>.Failure(error);
        }
    }
}
