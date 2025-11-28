using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PointOfSale.Entities;

namespace Poliedro.Eds.Domain.PosOfSale.DomainPosOfSale
{
    public interface IPosOfSaleGetAllService
    {
        Task<Result<IEnumerable<PosOfSaleEntity>, Error>> GetAllAsync(PaginationParams paginationParams);
    }
}
