using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.EdsTank.Entities;

namespace Poliedro.Eds.Domain.EdsTank.DomainEdsTank;
    public interface IEdsTankGetByIdEdsTank
    {
        Task<Result<EdsTankEntity, Error>> GetByIdAsync(int id);
    }