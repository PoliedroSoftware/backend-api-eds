using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;

namespace Poliedro.Eds.Domain.PosOfSaleDetails.DomainPosOfSaleDetails
{
    public interface IPosOfSaleDetailsGetByIdService
    {
        Task<Result<PosOfSaleDetailsEntity, Error>> GetByIdAsync(int id);
    }
}
