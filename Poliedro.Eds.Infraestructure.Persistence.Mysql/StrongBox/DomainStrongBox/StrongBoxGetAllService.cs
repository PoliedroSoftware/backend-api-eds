using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.StrongBox.Entities;
using Poliedro.Eds.Domain.StrongBox.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.EntityFrameworkCore;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.StrongBox.DomainStrongBox
{
    public class StrongBoxGetAllService(ITenantDbContextFactory dbContextFactory) : IStrongBoxRepositoryGetAll
    {
        public async Task<List<StrongBoxEntity>> GetListAsync(int skip, int take, long? IdCorte, string? type, DateTime? from, DateTime? to, CancellationToken cancellationToken)
        {
            using var db = dbContextFactory.CreateDbContext();
            return await db.Set<StrongBoxEntity>()
                .Where(x => (!IdCorte.HasValue || x.IdCorte == IdCorte) &&
                            (string.IsNullOrEmpty(type) || x.Type == type) &&
                            (!from.HasValue || x.DateTime >= from) &&
                            (!to.HasValue || x.DateTime <= to))
                .OrderByDescending(x => x.DateTime)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);
        }     
    }
}
