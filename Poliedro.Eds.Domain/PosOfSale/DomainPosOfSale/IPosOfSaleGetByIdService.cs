using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PointOfSale.Entities;

namespace Poliedro.Eds.Domain.PosOfSale.DomainPosOfSale
{
    public interface IPosOfSaleGetByIdService
    {
        Task<Result<PosOfSaleEntity, Error>> GetByIdAsync(int id);
    }
}
