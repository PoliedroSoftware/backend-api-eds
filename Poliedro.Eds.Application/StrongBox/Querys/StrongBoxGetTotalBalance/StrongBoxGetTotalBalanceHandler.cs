using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.StrongBox.Repositories;

namespace Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetTotalBalance;

public class StrongBoxGetTotalBalanceHandler(IStrongBoxRepositoryGetLast _repo) : IRequestHandler<StrongBoxGetTotalBalance, StrongBoxTotalBalanceDto>
{
    public async Task<StrongBoxTotalBalanceDto> Handle(StrongBoxGetTotalBalance request, CancellationToken cancellationToken)
    {
        var last = await _repo.GetLastAsync(cancellationToken);
        return new StrongBoxTotalBalanceDto
        {
            Id = last?.Id,
            DateTime = last?.CreatedAt,
            Saldo = last?.Saldo ?? 0.0
        };
    }
}
