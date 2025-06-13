using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Business.Entities;

namespace Poliedro.Eds.Domain.Business.DomainBusiness;

public interface IBusinessUpdateService
{
    Task<Result<VoidResult, Error>> UpdateAsync(BusinessEntity BusinessEntity);
}