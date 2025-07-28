using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Domain.Business.DomainBusiness;

public interface IBusinessGetAllService
{
    Task<IEnumerable<BusinessEntity>> GetAllAsync(PaginationParams paginationParams);
}
