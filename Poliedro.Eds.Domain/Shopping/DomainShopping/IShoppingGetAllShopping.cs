using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Shopping.Entities;

namespace Poliedro.Eds.Domain.Shopping.DomainShopping;
    public interface IShoppingGetAllShopping
    {
        Task<IEnumerable<ShoppingEntity>> GetAllAsync(PaginationParams paginationParams);
    }
