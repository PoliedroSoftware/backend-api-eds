using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.Wizard.Entities;

namespace Poliedro.Eds.Domain.Wizard.DomainSetup;

public interface ISetupCreateService
{
    Task<Result<VoidResult, Error>> CreateAsync(SetupEntity setupEntity);
}
