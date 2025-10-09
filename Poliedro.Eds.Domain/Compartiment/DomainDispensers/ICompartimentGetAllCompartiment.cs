using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.Compartiment.Entities;
namespace Poliedro.Eds.Domain.Compartiment.DomainCompartiment
{
    public interface ICompartimentGetAllService
    {
        Task<IEnumerable<CompartimentEntity>> GetAllAsync(PaginationParams paginationParams);
    }
}
