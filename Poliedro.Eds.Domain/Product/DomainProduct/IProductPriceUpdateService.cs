using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.Entities;

namespace Poliedro.Eds.Application.Product.Services;

public interface IProductPriceUpdateService
{
    Task<Result<VoidResult, Error>> UpdatePricesAsync(IEnumerable<ProductEntity> products);
}
