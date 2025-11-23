using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PointOfSale.Entities;

namespace Poliedro.Eds.Domain.PosOfSale.DomainPosOfSale
{
    public interface IPosOfSaleCreateService
    {
        Task<Result<VoidResult, Error>> CreateAsync(PosOfSaleEntity posOfSaleEntity);
    }
}
