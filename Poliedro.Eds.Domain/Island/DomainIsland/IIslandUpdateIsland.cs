using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Island.Entities;

namespace Poliedro.Eds.Domain.Island.DomainIsland
{
    public interface IIslandUpdateIsland
    {
        Task<Result<VoidResult, Error>> UpdateAsync(IslandEntity IslandEntity);
    }
}