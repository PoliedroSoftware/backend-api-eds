using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.Entities;

namespace Poliedro.Eds.Domain.Court.DomainService;

public interface ICourtGetByIdDomainService
{
    Task<Result<CourtEntity, Error>> GetByIdAsync(int id);

}
