using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.Entities;


namespace Poliedro.Eds.Domain.Compartiment.DomainCompartiment
{
    public interface ICompartimentCreateCompartiment
    {
        Task<Result<VoidResult, Error>> CreateAsync(CompartimentEntity compartimentEntity);

    }
}
