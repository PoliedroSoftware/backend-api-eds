using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductType.Entities;

namespace Poliedro.Eds.Domain.ProductType.DomainProductType
{
    public interface IProductTypeCreateProductType
    {
        Task<Result<VoidResult, Error>> CreateAsync(ProductTypeEntity ProductTypeEntity);
    }
}
