using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Domain.Category.DomainCategory;

public interface ICategoryGetAllService
{
    Task<IEnumerable<CategoryEntity>> GetAllAsync(PaginationParams paginationParams);
}
