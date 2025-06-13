using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.DispenserType.Entities;

namespace Poliedro.Eds.Domain.DispenserType.DomainDispenserType
{
    public interface IDispenserTypeGetByIdDispenserType
    {
        Task<Result<DispenserTypeEntity, Error>> GetByIdAsync(int id);
    }
}

