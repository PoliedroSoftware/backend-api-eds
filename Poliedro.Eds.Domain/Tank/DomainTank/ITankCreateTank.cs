using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Tank.Entities;

namespace Poliedro.Eds.Domain.Tank.DomainTank;
public interface ITankCreateTank
{
    Task<Result<VoidResult, Error>> CreateAsync(TankEntity TankEntity);
   
}

