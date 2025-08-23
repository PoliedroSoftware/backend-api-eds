using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.StrongBox.Entities;

namespace Poliedro.Eds.Domain.StrongBox.Repositories
{
    public interface IStrongBoxRepository
    {
        Task<StrongBoxEntity> GetByIdAsync(long id ,CancellationToken cancellationToken);
        Task<StrongBoxEntity> GetLastAsync(CancellationToken cancellationToken);
        Task<List<StrongBoxEntity>> GetListAsync(
            int skip,
            int take,
            long? IdCorte,
            string? type,
            DateTime? from,
            DateTime? to,
            CancellationToken cancellationToken);

        Task AddAsync(StrongBoxEntity entity, CancellationToken cancellationToken);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
