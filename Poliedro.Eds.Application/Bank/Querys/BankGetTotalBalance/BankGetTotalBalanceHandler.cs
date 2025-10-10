using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Bank.Dtos;
using Poliedro.Eds.Domain.Bank.Repositories;

namespace Poliedro.Eds.Application.Bank.Querys.BankGetTotalBalance;

public class BankGetTotalBalanceHandler(IBankRepositoryGetLast repository, IMapper mapper) : IRequestHandler<BankGetTotalBalance, BankDto?>
{
    public async Task<BankDto?> Handle(BankGetTotalBalance request, CancellationToken cancellationToken)
    {
        var lastEntry = await repository.GetLastByAccountAsync(request.IdAccount, cancellationToken);
        return mapper.Map<BankDto>(lastEntry) ?? null;
    }
}
