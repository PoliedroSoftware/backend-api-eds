using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.Dtos;

namespace Poliedro.Eds.Domain.Hose.DomainHose;

public interface IHoseGetAllHose
{
    Task<Result<IEnumerable<HoseDto>, Error>> GetAllAsync(PaginationParams paginationParams);
}
