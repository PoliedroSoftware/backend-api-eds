using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;

namespace Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
    public interface IShoppingProductUpdateShoppingProduct
    {
        Task<Result<VoidResult, Error>> UpdateAsync(ShoppingProductEntity shoppingProductEntity);
    }
