using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.DashboardPowerBI.ShoppingProductView.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.ShoppingProductView.DomainShoppingProductView;

public interface IShoppingProductGetAllShoppingProductView
{
    Task<IEnumerable<ShoppingProductViewEntity>> GetAllAsync(PaginationParams paginationParams);
}
