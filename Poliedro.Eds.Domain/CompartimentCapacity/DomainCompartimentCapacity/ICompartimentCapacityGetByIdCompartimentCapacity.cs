using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;

namespace Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;

public interface ICompartimentCapacityGetByIdService
{
    Task<Result<CompartimentCapacityEntity, Error>> GetByIdAsync(int id);
}
