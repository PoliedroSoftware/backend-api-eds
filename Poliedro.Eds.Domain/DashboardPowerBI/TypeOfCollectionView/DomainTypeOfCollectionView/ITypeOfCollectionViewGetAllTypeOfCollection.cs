using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;

namespace Poliedro.Eds.Domain.TypeOfCollection.DomainTypeOfCollection;
    public interface ITypeOfCollectionViewGetAllTypeOfCollection
    {
        Task<IEnumerable<TypeOfCollectionViewEntity>> GetAllAsync(PaginationParams paginationParams);
    }