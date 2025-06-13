using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Provider.Entities;

namespace Poliedro.Eds.Domain.Provider.DomainProvider;

public interface IProviderCreateService
{
    Task<Result<VoidResult, Error>> CreateAsync(ProviderEntity ProviderEntity);

}