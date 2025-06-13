using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.Entities;

namespace Poliedro.Eds.Domain.Islander.DomainIslander
{
    public interface IIslanderCreateIslander
    {
        Task<Result<VoidResult, Error>> CreateAsync(IslanderEntity IslanderEntity);
       
    }
}

