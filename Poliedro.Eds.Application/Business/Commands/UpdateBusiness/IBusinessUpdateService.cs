using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Business.Commands.UpdateBusiness
{
    public interface IBusinessUpdateService
    {
        Task<Result<VoidResult, Error>> UpdateAsync(BusinessEntity existingBusiness);
    }
}
