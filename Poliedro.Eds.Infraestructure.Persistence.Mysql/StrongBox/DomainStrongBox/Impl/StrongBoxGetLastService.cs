using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.StrongBox.Entities;
using Poliedro.Eds.Domain.StrongBox.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.StrongBox.DomainStrongBox;

public class StrongBoxGetLastService(ITenantDbContextFactory dbContextFactory) : IStrongBoxRepositoryGetLast
{
    public async Task<StrongBoxEntity?> GetLastAsync(CancellationToken cancellationToken)
    {
        using var db = dbContextFactory.CreateDbContext();

        return await db.Set<StrongBoxEntity>()
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenByDescending(x => x.Id)
                        .FirstOrDefaultAsync(cancellationToken);
    }
}
