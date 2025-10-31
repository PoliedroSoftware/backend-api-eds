using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.StrongBox.Entities;
using Poliedro.Eds.Domain.StrongBox.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.StrongBox.DomainStrongBox;

public class StrongBoxGetByEdsService(ITenantDbContextFactory dbContextFactory, ILogger<StrongBoxGetByEdsService> logger) : IStrongBoxRepositoryGetByEds
{
    public async Task<List<StrongBoxEntity>> GetByEdsAsync(int idEds, CancellationToken cancellationToken)
    {
        using var db = dbContextFactory.CreateDbContext();
        return await db.Set<StrongBoxEntity>()
            .AsNoTracking()
            .Where(x => x.IdEds == idEds)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
