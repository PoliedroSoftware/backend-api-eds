using Poliedro.Eds.Domain.Bank.Entities;

namespace Poliedro.Eds.Domain.Bank.Repositories;

public interface IBankRepositoryCreate
{
    Task CreateAsync(BankEntity entity, CancellationToken cancellationToken);
}
