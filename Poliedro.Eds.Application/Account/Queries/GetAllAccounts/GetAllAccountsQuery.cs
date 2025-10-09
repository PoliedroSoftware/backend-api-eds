using MediatR;
using Poliedro.Eds.Application.Account.Dtos;

namespace Poliedro.Eds.Application.Account.Queries.GetAllAccounts;

public record GetAllAccountsQuery : IRequest<IEnumerable<AccountDto>>;
