using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DispenserType.Entities;

namespace Poliedro.Eds.Domain.DispenserType.DomainDispenserType
{
    public interface IDispenserTypeGetAllDispenserType
    {
        Task<IEnumerable<DispenserTypeEntity>> GetAllAsync(PaginationParams paginationParams);
    }
}