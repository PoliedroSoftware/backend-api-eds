using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Domain.Court.DomainService;

public interface ICourtListDomainService
{
    Task<IEnumerable<CourtListResponseEntity>> GetAllAsync(PaginationParams paginationParams, string username, bool isAdmin);
}
