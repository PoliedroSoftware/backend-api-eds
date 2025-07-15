using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.ShoppingProductView.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.ShoppingProductView.DomainShoppingProductView;
    public interface IShoppingProductGetAllShoppingProductView
    {
        Task<IEnumerable<ShoppingProductViewEntity>> GetAllAsync(PaginationParams paginationParams);
    }
