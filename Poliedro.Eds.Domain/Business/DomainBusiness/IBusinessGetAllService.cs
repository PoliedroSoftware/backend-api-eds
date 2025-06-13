using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Business.Entities;

namespace Poliedro.Eds.Domain.Business.DomainBusiness;

public interface IBusinessGetAllService
{
    Task<IEnumerable<BusinessEntity>> GetAllAsync(PaginationParams paginationParams);
}