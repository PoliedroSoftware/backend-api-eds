using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.Entities;

namespace Poliedro.Eds.Domain.Court.DomainService
{
    public interface ICourtDomainService
    {
        Task<Result<VoidResult, Error>> CreateAsync(CourtEntity courtEntity);

    }
}
