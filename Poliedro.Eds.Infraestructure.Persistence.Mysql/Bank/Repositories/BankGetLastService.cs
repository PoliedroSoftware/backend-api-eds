using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Bank.Entities;
using Poliedro.Eds.Domain.Bank.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Bank.Repositories;

public class BankGetLastService(ITenantDbContextFactory dbContextFactory) : IBankRepositoryGetLast
{
    public async Task<BankEntity?> GetLastByAccountAsync(int idAccount, CancellationToken cancellationToken)
    {
        using var db = dbContextFactory.CreateDbContext();

        return await db.Set<BankEntity>()
                        .Where(x => x.IdAccount == idAccount)
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenByDescending(x => x.IdBank)
                        .FirstOrDefaultAsync(cancellationToken);
    }
}
