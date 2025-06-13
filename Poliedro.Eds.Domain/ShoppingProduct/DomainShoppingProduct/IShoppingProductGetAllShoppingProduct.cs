using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;

namespace Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
    public interface IShoppingProductGetAllShoppingProduct
    {
        Task<IEnumerable<ShoppingProductEntity>> GetAllAsync(PaginationParams paginationParams);
    }
