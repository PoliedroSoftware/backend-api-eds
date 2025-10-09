using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Account.Dtos;
using Poliedro.Eds.Domain.Account.Services;

namespace Poliedro.Eds.Application.Account.Queries.GetAccountById;

public class GetAccountByIdQueryHandler(
    IAccountGetByIdService accountGetByIdService,
    IMapper mapper) : IRequestHandler<GetAccountByIdQuery, AccountDto?>
{
    public async Task<AccountDto?> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await accountGetByIdService.GetByIdAsync(request.Id, cancellationToken);
        return mapper.Map<AccountDto>(account);
    }
}
