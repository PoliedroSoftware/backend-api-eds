using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.StrongBox.Entities;
using Poliedro.Eds.Domain.StrongBox.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.StrongBox.Repositories
{
    public class StrongBoxCreateService(ITenantDbContextFactory dbContextFactory) : IStrongBoxRepositoryCreate
    {
        public async Task CreateAsync(StrongBoxEntity entity, CancellationToken cancellationToken)
        {
            using var db = dbContextFactory.CreateDbContext();
            await db.AddAsync(entity, cancellationToken);
        }       
    }
}
