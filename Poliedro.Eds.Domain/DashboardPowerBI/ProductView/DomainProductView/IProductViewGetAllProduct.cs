using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.ProductView.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.ProductView.DomainProductView;

public interface IProductViewGetAllProduct
{
    Task<IEnumerable<ProductViewEntity>> GetAllAsync(PaginationParams paginationParams);
}
