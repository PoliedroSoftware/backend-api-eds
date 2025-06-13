using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Dispensers.Entities;

namespace Poliedro.Eds.Domain.Dispensers.DomainDispensers
{
    public interface IDispensersUpdateDispensers
    {
        Task<Result<VoidResult, Error>> UpdateAsync(DispensersEntity dispensersEntity);

    }
}