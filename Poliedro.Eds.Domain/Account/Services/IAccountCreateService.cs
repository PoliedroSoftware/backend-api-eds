using Poliedro.Eds.Domain.Account.Entities;

namespace Poliedro.Eds.Domain.Account.Services;

public interface IAccountCreateService
{
    Task CreateAsync(AccountEntity entity, CancellationToken cancellationToken);
}
