using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.StrongBox.Entities;

namespace Poliedro.Eds.Domain.StrongBox.Services;

public interface IStrongBoxService
{
    Task<StrongBoxEntity> CreateAsync(
        long? idCorte,
        int? idEds,
        string type,
        double ammount,
        string? note,
        CancellationToken cancellationToken);
}
