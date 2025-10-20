using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.TransferValidation.Entities;
using Poliedro.Eds.Domain.TransferValidation.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.TransferValidation.Repositories;

public class TransferValidationRepositoryGetById(ITenantDbContextFactory dbContextFactory) 
    : ITransferValidationRepositoryGetById
{
    public async Task<TransferValidationEntity?> GetByIdAsync(
        int idTransferValidation, 
        CancellationToken cancellationToken = default)
    {
        using var context = dbContextFactory.CreateDbContext();
        
        return await context.TransferValidation
            .AsNoTracking()
            .FirstOrDefaultAsync(tv => tv.IdTransferValidation == idTransferValidation, cancellationToken);
    }
}
