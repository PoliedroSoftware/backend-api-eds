using Poliedro.Eds.Domain.Account.Entities;

namespace Poliedro.Eds.Domain.Account.Services;

public interface IAccountGetByIdService
{
    Task<AccountEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
