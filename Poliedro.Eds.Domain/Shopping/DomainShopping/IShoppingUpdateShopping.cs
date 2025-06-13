using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Shopping.Entities;

namespace Poliedro.Eds.Domain.Shopping.DomainShopping;
    public interface IShoppingUpdateShopping
    {
        Task<Result<VoidResult, Error>> UpdateAsync(ShoppingEntity shoppingEntity);
    }
