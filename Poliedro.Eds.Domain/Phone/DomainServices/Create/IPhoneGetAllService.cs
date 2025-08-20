using Poliedro.Eds.Domain.Phone.Entities;
using Poliedro.Eds.Domain.Common.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poliedro.Eds.Domain.Phone.DomainServices.GetAll;

public interface IPhoneGetAllService
{
    Task<IEnumerable<PhoneEntity>> GetAllAsync(PaginationParams paginationParams);
}
