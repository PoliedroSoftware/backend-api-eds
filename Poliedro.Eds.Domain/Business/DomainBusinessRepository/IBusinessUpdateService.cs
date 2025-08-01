using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Domain.Business.DomainBusiness;

public interface IBusinessUpdateService
{
    Task<Result<VoidResult, Error>> UpdateAsync(BusinessEntity businessEntity);
    Task<BusinessEntity?> GetByIdAsync(int id); 
}

