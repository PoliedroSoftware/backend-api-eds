using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.StrongBox.Entities;

namespace Poliedro.Eds.Domain.StrongBox.Repositories
{
    public interface IStrongBoxRepositoryGetAll
    {
        Task<List<StrongBoxEntity>> GetListAsync(
            int skip,
            int take,
            long? IdCorte,
            string? type,
            DateTime? from,
            DateTime? to,
            CancellationToken cancellationToken);
    }
}
