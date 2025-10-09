using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.Island.Entities;

namespace Poliedro.Eds.Domain.Island.DomainIsland
{
    public interface IIslandGetAllIsland
    {
        Task<IEnumerable<IslandEntity>> GetAllAsync(PaginationParams paginationParams);
    }
}
