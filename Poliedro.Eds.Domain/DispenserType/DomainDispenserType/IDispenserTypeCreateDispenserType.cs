
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.DispenserType.Entities;


namespace Poliedro.Eds.Domain.DispenserType.DomainDispenserType
{
    public interface IDispenserTypeCreateDispenserType
    {
        Task<Result<VoidResult, Error>> CreateAsync(DispenserTypeEntity dispenserTypeEntity);

    }
}
