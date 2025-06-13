using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Domain.Eds.DomainEds;

public interface IEdsGetByIdService
{
    Task<Result<EdsEntity, Error>> GetByIdAsync(int id);
}