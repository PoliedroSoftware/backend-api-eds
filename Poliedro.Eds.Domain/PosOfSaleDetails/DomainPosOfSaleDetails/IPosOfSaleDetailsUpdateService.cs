using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;

namespace Poliedro.Eds.Domain.PosOfSaleDetails.DomainPosOfSaleDetails
{
    public interface IPosOfSaleDetailsUpdateService
    {
        Task<Result<VoidResult, Error>> UpdateAsync(PosOfSaleDetailsEntity posOfSaleEntity);
    }
}
