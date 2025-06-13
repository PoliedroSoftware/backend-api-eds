using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.Entities;

namespace Poliedro.Eds.Domain.Compartiment.DomainCompartiment
{
    public interface ICompartimentUpdateCompartiment
    {
        Task<Result<VoidResult, Error>> UpdateAsync(CompartimentEntity compartimentEntity);

    }
}