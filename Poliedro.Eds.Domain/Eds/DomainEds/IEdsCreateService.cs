using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Domain.Eds.DomainEds;

public interface IEdsCreateService
{
    Task<Result<VoidResult, Error>> CreateAsync(EdsEntity EdsEntity);

}