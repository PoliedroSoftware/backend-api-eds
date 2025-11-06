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

public class StrongBoxGetLastService(ITenantDbContextFactory dbContextFactory, ILogger<StrongBoxGetLastService> logger) : IStrongBoxRepositoryGetLast
{
    public async Task<StrongBoxEntity?> GetLastAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting StrongBox");
        using var db = dbContextFactory.CreateDbContext();

        return await db.Set<StrongBoxEntity>()
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenByDescending(x => x.Id)
                        .FirstOrDefaultAsync(cancellationToken);
    }
}
