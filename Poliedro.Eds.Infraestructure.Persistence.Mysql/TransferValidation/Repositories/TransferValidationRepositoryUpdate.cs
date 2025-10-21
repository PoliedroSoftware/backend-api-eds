using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.TransferValidation.Entities;
using Poliedro.Eds.Domain.TransferValidation.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.TransferValidation.Repositories;

public class TransferValidationRepositoryUpdate(ITenantDbContextFactory dbContextFactory) 
    : ITransferValidationRepositoryUpdate
{
    public async Task<TransferValidationEntity> UpdateAsync(
        TransferValidationEntity entity, 
        CancellationToken cancellationToken = default)
    {
        using var context = dbContextFactory.CreateDbContext();
        
        context.TransferValidation.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        
        return entity;
    }
}
