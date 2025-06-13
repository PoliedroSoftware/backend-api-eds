using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;

namespace Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
    public interface IProductCompartimentUpdateProductCompartiment
    {
        Task<Result<VoidResult, Error>> UpdateAsync(ProductCompartimentEntity ServerEntity); 
    }
