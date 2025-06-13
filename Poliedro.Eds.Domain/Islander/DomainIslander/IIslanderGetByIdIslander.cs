using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.Entities;

namespace Poliedro.Eds.Domain.Islander.DomainIslander
{
    public interface IIslanderGetByIdIslander
    {
        Task<Result<IslanderEntity, Error>> GetByIdAsync(int id);   
    }
}

