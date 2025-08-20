using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Domain.Shopping.Entities;

namespace Poliedro.Eds.Domain.Shopping.DomainShopping;

public interface IShoppingTransactionalService
{
    Task<Result<VoidResult, Error>> ExecuteShoppingTransactionAsync(ShoppingEntity shoppingEntity, IEnumerable<ProductEntity> productsToUpdatePrice);
}
