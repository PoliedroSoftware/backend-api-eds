using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.Entities;

namespace Poliedro.Eds.Domain.Product.DomainProduct;

    public interface IProductCreateProduct
    {
        Task<Result<VoidResult, Error>> CreateAsync(ProductEntity ProductEntity);
    }
