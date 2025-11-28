using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;

namespace Poliedro.Eds.Domain.PosOfSaleDetails.DomainPosOfSaleDetails
{
    public interface IPosOfSaleDetailsGetAllService
    {
        Task<Result<IEnumerable<PosOfSaleDetailsEntity>, Error>> GetAllAsync(PaginationParams paginationParams);
    }
}
