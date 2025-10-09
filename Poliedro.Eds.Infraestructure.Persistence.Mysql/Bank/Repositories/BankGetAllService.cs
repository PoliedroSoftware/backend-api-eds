using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Bank.Entities;
using Poliedro.Eds.Domain.Bank.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Bank.Repositories;

public class BankGetAllService(ITenantDbContextFactory dbContextFactory) : IBankRepositoryGetAll
{
    public async Task<IEnumerable<BankEntity>> GetAllAsync(int? idAccount = null, int? idEds = null, CancellationToken cancellationToken = default)
    {
        using var db = dbContextFactory.CreateDbContext();
        
        var query = db.Set<BankEntity>().AsQueryable();

        if (idAccount.HasValue)
        {
            query = query.Where(b => b.IdAccount == idAccount.Value);
        }

        if (idEds.HasValue)
        {
            query = query.Where(b => b.IdEds == idEds.Value);
        }

        return await query
                    .OrderByDescending(x => x.IdBank) // Cambiar a solo IdBank para evitar problemas con CreatedAt
                    .ToListAsync(cancellationToken);
    }
}
