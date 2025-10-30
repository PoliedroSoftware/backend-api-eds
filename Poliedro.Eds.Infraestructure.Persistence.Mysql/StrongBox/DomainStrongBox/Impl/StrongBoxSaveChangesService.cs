using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.StrongBox.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.StrongBox.DomainStrongBox;

public class StrongBoxSaveChangesService(ITenantDbContextFactory dbContextFactory, ILogger<StrongBoxSaveChangesService> logger) : IStrongBoxRepositorySaveChanges
{
    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing StrongBox");
        using var db = dbContextFactory.CreateDbContext();
        return await db.SaveChangesAsync(cancellationToken) > 0;
    }
}
