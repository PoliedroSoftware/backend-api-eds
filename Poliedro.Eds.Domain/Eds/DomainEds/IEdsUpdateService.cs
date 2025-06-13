using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Domain.Eds.DomainEds;

public interface IEdsUpdateService
{
    Task<Result<VoidResult, Error>> UpdateAsync(EdsEntity EdsEntity);
}