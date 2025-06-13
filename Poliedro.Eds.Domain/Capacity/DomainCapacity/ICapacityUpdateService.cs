using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Capacity.Entities;

namespace Poliedro.Eds.Domain.Capacity.DomainCapacity;

public interface ICapacityUpdateService
{
    Task<Result<VoidResult, Error>> UpdateAsync(CapacityEntity CapacityEntity);
}