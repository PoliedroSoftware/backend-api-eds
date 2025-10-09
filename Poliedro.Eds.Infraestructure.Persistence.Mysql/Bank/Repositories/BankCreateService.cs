using Poliedro.Eds.Domain.Bank.Entities;
using Poliedro.Eds.Domain.Bank.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Bank.Repositories;

public class BankCreateService(ITenantDbContextFactory dbContextFactory) : IBankRepositoryCreate
{
    public async Task CreateAsync(BankEntity entity, CancellationToken cancellationToken)
    {
        using var db = dbContextFactory.CreateDbContext();
        await db.AddAsync(entity, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }
}
