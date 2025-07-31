using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Domain.Capacity.DomainCapacity;

public interface ICapacityCreateService
{
    Task<Result<VoidResult, Error>> CreateAsync(CapacityEntity CapacityEntity);

}
