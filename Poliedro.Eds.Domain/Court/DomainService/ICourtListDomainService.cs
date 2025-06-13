using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Domain.Court.DomainService;
public interface ICourtListDomainService
{
    Task<IEnumerable<CourtListResponseDto>> GetAllAsync(PaginationParams paginationParams);
}
