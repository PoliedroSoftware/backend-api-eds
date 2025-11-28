using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;

namespace Poliedro.Eds.Domain.PosOfSaleDetails.DomainPosOfSaleDetails
{
    public interface IPosOfSaleDetailsCreateService
    {
        Task<Result<VoidResult, Error>> CreateAsync(PosOfSaleDetailsEntity posOfSaleEntity);
    }
}
