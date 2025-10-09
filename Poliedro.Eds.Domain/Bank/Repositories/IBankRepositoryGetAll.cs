using Poliedro.Eds.Domain.Bank.Entities;

namespace Poliedro.Eds.Domain.Bank.Repositories;

public interface IBankRepositoryGetAll
{
    Task<IEnumerable<BankEntity>> GetAllAsync(int? idAccount = null, int? idEds = null, CancellationToken cancellationToken = default);
}
