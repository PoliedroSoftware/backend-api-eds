using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Bank.Entities;
using Poliedro.Eds.Domain.Bank.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Bank.Repositories;

public class BankGetByIdService(ITenantDbContextFactory dbContextFactory) : IBankRepositoryGetById
{
    public async Task<BankEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        using var db = dbContextFactory.CreateDbContext();

        return await db.Set<BankEntity>()
                        .FirstOrDefaultAsync(x => x.IdBank == id, cancellationToken);
    }
}
