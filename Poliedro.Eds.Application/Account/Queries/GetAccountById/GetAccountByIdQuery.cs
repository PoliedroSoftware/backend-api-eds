using MediatR;
using Poliedro.Eds.Application.Account.Dtos;

namespace Poliedro.Eds.Application.Account.Queries.GetAccountById;

public record GetAccountByIdQuery(int Id) : IRequest<AccountDto?>;
