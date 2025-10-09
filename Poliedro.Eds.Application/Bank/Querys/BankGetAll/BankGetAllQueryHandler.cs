using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Bank.Dtos;
using Poliedro.Eds.Domain.Bank.Repositories;

namespace Poliedro.Eds.Application.Bank.Querys.BankGetAll;

public class BankGetAllQueryHandler(
    IBankRepositoryGetAll repository,
    IMapper mapper) : IRequestHandler<BankGetAllQuery, IEnumerable<BankDto>>
{
    public async Task<IEnumerable<BankDto>> Handle(BankGetAllQuery request, CancellationToken cancellationToken)
    {
        var banks = await repository.GetAllAsync(request.IdAccount, request.IdEds, cancellationToken);
        return mapper.Map<IEnumerable<BankDto>>(banks);
    }
}
