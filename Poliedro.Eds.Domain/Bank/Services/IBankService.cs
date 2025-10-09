using Poliedro.Eds.Domain.Bank.Entities;

namespace Poliedro.Eds.Domain.Bank.Services;

public interface IBankService
{
    Task<BankEntity> CreateAsync(
        int idAccount,
        int idEds,
        int? idCourt,
        string moviment,
        double ammount,
        string note,
        CancellationToken cancellationToken);
}
