using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Compartiment.Entities;
namespace Poliedro.Eds.Domain.Compartiment.DomainCompartiment
{
    public interface ICompartimentGetAllCompartiment
    {
        Task<IEnumerable<CompartimentEntity>> GetAllAsync(PaginationParams paginationParams);
    }
}