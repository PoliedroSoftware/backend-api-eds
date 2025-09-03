using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.StrongBox.Entities;

namespace Poliedro.Eds.Domain.StrongBox.Repositories;

public interface IStrongBoxRepositoryGetById
{
    Task<StrongBoxEntity?> GetByIdAsync(long id, CancellationToken cancellationToken);
}
