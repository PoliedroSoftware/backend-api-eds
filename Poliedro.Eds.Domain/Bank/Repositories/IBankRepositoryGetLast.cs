using Poliedro.Eds.Domain.Bank.Entities;

namespace Poliedro.Eds.Domain.Bank.Repositories;

public interface IBankRepositoryGetLast
{
    Task<BankEntity?> GetLastByAccountAsync(int idAccount, CancellationToken cancellationToken);
}
