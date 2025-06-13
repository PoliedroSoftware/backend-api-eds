
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Shopping.Entities;


namespace Poliedro.Eds.Domain.Shopping.DomainShopping;
    public interface IShoppingCreateShopping
    {
        Task<Result<VoidResult, Error>> CreateAsync(ShoppingEntity shoppingEntity);
    }

