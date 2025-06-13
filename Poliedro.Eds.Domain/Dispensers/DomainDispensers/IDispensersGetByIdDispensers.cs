using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Dispensers.Entities;

namespace Poliedro.Eds.Domain.Dispensers.DomainDispensers
{
    public interface IDispensersGetByIdDispensers
    {
        Task<Result<DispensersEntity, Error>> GetByIdAsync(int id);
    }
}

