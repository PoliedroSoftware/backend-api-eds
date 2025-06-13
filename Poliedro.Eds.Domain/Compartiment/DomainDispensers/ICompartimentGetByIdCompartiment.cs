using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.Entities;

namespace Poliedro.Eds.Domain.Compartiment.DomainCompartiment
{
    public interface ICompartimentGetByIdCompartiment
    {
        Task<Result<CompartimentEntity, Error>> GetByIdAsync(int id);
    }
}

