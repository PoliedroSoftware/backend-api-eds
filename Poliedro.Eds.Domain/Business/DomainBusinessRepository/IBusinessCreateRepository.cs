using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Domain.Business.DomainBusiness;

public interface IBusinessCreateRepository
{
    Task<Result<VoidResult, Error>> CreateAsync(BusinessEntity BusinessEntity);
}
