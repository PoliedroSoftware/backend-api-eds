using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Account.Dtos;
using Poliedro.Eds.Domain.Account.Services;

namespace Poliedro.Eds.Application.Account.Queries.GetAllAccounts;

public class GetAllAccountsQueryHandler(
    IAccountGetAllService accountGetAllService,
    IMapper mapper) : IRequestHandler<GetAllAccountsQuery, IEnumerable<AccountDto>>
{
    public async Task<IEnumerable<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await accountGetAllService.GetAllAsync();
        return mapper.Map<IEnumerable<AccountDto>>(accounts);
    }
}
