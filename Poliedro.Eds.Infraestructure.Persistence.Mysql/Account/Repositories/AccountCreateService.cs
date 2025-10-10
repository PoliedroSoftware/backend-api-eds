using Poliedro.Eds.Domain.Account.Entities;
using Poliedro.Eds.Domain.Account.Services;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Account.Repositories;

public class AccountCreateService(ITenantDbContextFactory dbContextFactory) : IAccountCreateService
{
    public async Task CreateAsync(AccountEntity entity, CancellationToken cancellationToken)
    {
        using var db = dbContextFactory.CreateDbContext();
        await db.AddAsync(entity, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }
}
