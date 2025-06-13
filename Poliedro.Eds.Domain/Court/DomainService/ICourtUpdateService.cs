using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.Entities;

namespace Poliedro.Eds.Domain.Court.DomainService
{
    public interface ICourtUpdateService
    {

        Task<Result<VoidResult, Error>> UpdateAsync(CourtEntity courtEntity);

        
    }
}
