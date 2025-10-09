using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Account.Entities;
using Poliedro.Eds.Domain.Account.Services;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Account.Repositories;

public class AccountGetByIdService(ITenantDbContextFactory dbContextFactory) : IAccountGetByIdService
{
    public async Task<AccountEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        using var db = dbContextFactory.CreateDbContext();
        return await db.Set<AccountEntity>()
                      .FirstOrDefaultAsync(x => x.IdAccount == id, cancellationToken);
    }
}
