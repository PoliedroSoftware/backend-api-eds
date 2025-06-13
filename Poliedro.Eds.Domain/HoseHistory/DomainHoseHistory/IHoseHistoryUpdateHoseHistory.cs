using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.HoseHistory.Entities;

namespace Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory
{
    public interface IHoseHistoryUpdateHoseHistory
    {
        Task<Result<VoidResult, Error>> UpdateAsync(HoseHistoryEntity HoseHistoryEntity);
    }
}