using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Business.Entities;

namespace Poliedro.Eds.Domain.Business.DomainBusiness;

public interface IBusinessCreateService
{
    Task<Result<VoidResult, Error>> CreateAsync(BusinessEntity BusinessEntity);

}