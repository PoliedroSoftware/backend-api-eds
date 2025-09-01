using Poliedro.Eds.Domain.Common.Pagination;
<<<<<<< HEAD
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
=======
>>>>>>> New-service-StrongBox
using Poliedro.Eds.Domain.Hose.Dtos;

namespace Poliedro.Eds.Domain.Hose.DomainHose;

public interface IHoseGetAllHose
{
<<<<<<< HEAD
    Task<Result<IEnumerable<HoseDto>, Error>> GetAllAsync(PaginationParams paginationParams);
=======
    Task<IEnumerable<HoseDto>> GetAllAsync(PaginationParams paginationParams);
>>>>>>> New-service-StrongBox
}
