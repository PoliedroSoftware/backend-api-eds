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
            if (from.HasValue && to.HasValue && from > to)
            {
                var tmp = from.Value; from = to; to = tmp;
            }

            var query = db.Set<StrongBoxEntity>().AsNoTracking();

            if (IdCorte.HasValue) query = query.Where(x => x.IdCorte == IdCorte);
            if (!string.IsNullOrWhiteSpace(type)) query = query.Where(x => x.Type == type);
            if (from.HasValue) query = query.Where(x => x.DateTime >= from.Value);
            if (to.HasValue) query = query.Where(x => x.DateTime <= to.Value);

            return await query
                .OrderByDescending(x => x.DateTime)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);
        }
    }
}
