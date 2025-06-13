using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.Entities;

namespace Poliedro.Eds.Domain.Hose.DomainHose
{
    public interface IHoseCreateHose
    {
        Task<Result<VoidResult, Error>> CreateAsync(HoseEntity HoseEntity);
       
    }
}

