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
    public class StrongBoxRepository(DataBaseContext context) : IStrongBoxRepository
    {
        public Task AddAsync(StrongBoxEntity entity, CancellationToken cancellationToken)
        {
            return context.AddAsync(entity, cancellationToken).AsTask();
        }

        public Task<StrongBoxEntity> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return context.Set<StrongBoxEntity>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        
        public Task<StrongBoxEntity> GetLastAsync(CancellationToken cancellationToken)
        {
            return context.Set<StrongBoxEntity>()
                .OrderByDescending(x => x.DateTime)
                .ThenByDescending(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<StrongBoxEntity>> GetListAsync(int skip, int take, long? IdCorte, string? type, DateTime? from, DateTime? to, CancellationToken cancellationToken)
        {
            var query = context.Set<StrongBoxEntity>().AsQueryable();
            if (IdCorte is not null) query = query.Where(x => x.IdCorte == IdCorte);
            if (!string.IsNullOrWhiteSpace(type))
            {
                var types = type.Trim().ToUpperInvariant();
                query = query.Where(x => x.Type == types || x.Type == types);
            }

            if (from is not null) query = query.Where(x => x.DateTime >= from);
            if (to is not null) query = query.Where(x => x.DateTime <= to);

            return await query.OrderByDescending(x => x.DateTime)
                .ThenByDescending(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
