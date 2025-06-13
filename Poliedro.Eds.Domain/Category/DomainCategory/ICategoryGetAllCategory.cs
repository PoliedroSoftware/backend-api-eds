using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Domain.Category.DomainCategory;
    public interface ICategoryGetAllCategory
{
        Task<IEnumerable<CategoryEntity>> GetAllAsync(PaginationParams paginationParams);
    }
