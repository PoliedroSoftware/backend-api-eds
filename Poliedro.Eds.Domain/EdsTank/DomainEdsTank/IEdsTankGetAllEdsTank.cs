using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.EdsTank.Entities;

namespace Poliedro.Eds.Domain.EdsTank.DomainEdsTank;

    public interface IEdsTankGetAllEdsTank
    {
        Task<IEnumerable<EdsTankEntity>> GetAllAsync(PaginationParams paginationParams);
    }