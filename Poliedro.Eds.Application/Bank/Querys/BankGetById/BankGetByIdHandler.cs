using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Bank.Dtos;
using Poliedro.Eds.Domain.Bank.Repositories;

namespace Poliedro.Eds.Application.Bank.Querys.BankGetById;

public class BankGetByIdHandler(IBankRepositoryGetById repository, IMapper mapper) : IRequestHandler<BankGetId, BankDto?>
{
    public async Task<BankDto?> Handle(BankGetId request, CancellationToken cancellationToken)
        => mapper.Map<BankDto>(await repository.GetByIdAsync(request.Id, cancellationToken)) ?? null;
}
