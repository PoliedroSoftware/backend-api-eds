using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.ShoppingProductView.Entities;

namespace Poliedro.Eds.Domain.ShoppingProductView.DomainShoppingProductView;
    public interface IShoppingProductGetAllShoppingProductView
    {
        Task<IEnumerable<ShoppingProductViewEntity>> GetAllAsync(PaginationParams paginationParams);
    }
