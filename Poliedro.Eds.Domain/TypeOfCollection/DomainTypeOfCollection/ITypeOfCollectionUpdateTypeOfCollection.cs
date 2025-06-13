using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;

namespace Poliedro.Eds.Domain.TypeOfCollection.DomainTypeOfCollection;

    public interface ITypeOfCollectionUpdateTypeOfCollection
    {
        Task<Result<VoidResult, Error>> UpdateAsync(TypeOfCollectionEntity ServerEntity); 
    }