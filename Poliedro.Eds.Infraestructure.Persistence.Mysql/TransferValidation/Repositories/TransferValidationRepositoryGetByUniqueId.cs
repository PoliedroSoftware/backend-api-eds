using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.TransferValidation.Entities;
using Poliedro.Eds.Domain.TransferValidation.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.TransferValidation.Repositories;

public class TransferValidationRepositoryGetByUniqueId(ITenantDbContextFactory dbContextFactory)
    : ITransferValidationRepositoryGetByUniqueId
{
    public async Task<TransferValidationEntity?> GetByUniqueIdAsync(
        string uniqueId,
        CancellationToken cancellationToken = default)
    {
        using var context = dbContextFactory.CreateDbContext();
        
        return await context.TransferValidation
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UniqueId == uniqueId, cancellationToken);
    }
}
