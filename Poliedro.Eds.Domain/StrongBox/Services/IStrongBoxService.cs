using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.StrongBox.Entities;

namespace Poliedro.Eds.Domain.StrongBox.Services
{
    public interface IStrongBoxService
    {
        Task<StrongBoxEntity> CreateAsync(
            DateTime dateTime, long? idCorte, string type, decimal ammount, string? note, string createdBy,
            CancellationToken cancellationToken);
    }
}
