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

public class StrongBoxGetByIdService(ITenantDbContextFactory dbContextFactory, ILogger<StrongBoxGetByIdService> logger) : IStrongBoxRepositoryGetById
{
    public async Task<StrongBoxEntity> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting StrongBox");
        using var db = dbContextFactory.CreateDbContext();
        return await db.Set<StrongBoxEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
