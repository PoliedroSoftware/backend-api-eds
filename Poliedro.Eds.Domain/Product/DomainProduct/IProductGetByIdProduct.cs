using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.Entities;

namespace Poliedro.Eds.Domain.Product.DomainProduct;
    public interface IProductGetByIdProduct
    {
        Task<Result<ProductEntity, Error>> GetByIdAsync(int id);
    }