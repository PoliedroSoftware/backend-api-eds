using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Tank.Entities;

namespace Poliedro.Eds.Domain.Tank.DomainTank;
public interface ITankGetByIdTank
{
    Task<Result<TankEntity, Error>> GetByIdAsync(int id);   
}

