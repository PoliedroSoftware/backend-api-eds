using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Domain.StrongBox.Repositories;

public interface IStrongBoxRepositorySaveChanges
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
