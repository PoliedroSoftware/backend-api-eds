using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.StrongBox.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.StrongBox.DomainStrongBox
{
    internal class StrongBoxSaveChangesService(ITenantDbContextFactory dbContextFactory) : IStrongBoxRepositorySaveChanges
    {
        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            using var db = dbContextFactory.CreateDbContext();
            return await db.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
