using Poliedro.Eds.Domain.Bank.Entities;

namespace Poliedro.Eds.Domain.Bank.Repositories;

public interface IBankRepositoryGetById
{
    Task<BankEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
