using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Domain.Business.DomaianServices.Create;

public interface IBusinessCreateDomianService
{
    Task<Result<VoidResult, Error>> CreateAsync(BusinessEntity business);
}
