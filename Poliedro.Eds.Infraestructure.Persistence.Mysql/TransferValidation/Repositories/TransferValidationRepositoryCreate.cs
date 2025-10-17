using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.TransferValidation.Entities;
using Poliedro.Eds.Domain.TransferValidation.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.TransferValidation.Repositories;

public class TransferValidationRepositoryCreate(ITenantDbContextFactory dbContextFactory) 
    : ITransferValidationRepositoryCreate
{
    public async Task<TransferValidationEntity> CreateAsync(
        TransferValidationEntity entity,
        CancellationToken cancellationToken = default)
    {
        using var context = dbContextFactory.CreateDbContext();
        
        await context.TransferValidation.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return entity;
    }
}
