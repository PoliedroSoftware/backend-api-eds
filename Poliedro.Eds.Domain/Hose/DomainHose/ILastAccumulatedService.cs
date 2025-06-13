using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.Entities;

namespace Poliedro.Eds.Domain.Hose.DomainHose;

public interface ILastAccumulatedService
{
    Task<Result<LastAccumulatedEntity, Error>> GetLastAccumulatedAsync(int idDispenser, int idHose);
}