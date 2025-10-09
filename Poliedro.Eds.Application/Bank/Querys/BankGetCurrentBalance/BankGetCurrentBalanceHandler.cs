using MediatR;
using Poliedro.Eds.Domain.Bank.Repositories;

namespace Poliedro.Eds.Application.Bank.Querys.BankGetCurrentBalance;

public class BankGetCurrentBalanceHandler(IBankRepositoryGetLast repository) : IRequestHandler<BankGetCurrentBalance, double>
{
    public async Task<double> Handle(BankGetCurrentBalance request, CancellationToken cancellationToken)
    {
        var lastEntry = await repository.GetLastByAccountAsync(request.IdAccount, cancellationToken);
        return lastEntry?.Balance ?? 0.0;
    }
}
