using System.Collections.Generic;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;

namespace Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;

public interface IProductCompartimentStockUpdate
{
    Task<Result<VoidResult, Error>> UpdateStockAsync(IEnumerable<ShoppingProductEntity> shoppingProducts);
}
