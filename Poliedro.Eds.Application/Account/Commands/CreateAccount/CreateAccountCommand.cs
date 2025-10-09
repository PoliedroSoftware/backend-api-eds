using MediatR;
using Poliedro.Eds.Application.Account.Dtos;

namespace Poliedro.Eds.Application.Account.Commands.CreateAccount;

public record CreateAccountCommand(AccountCreateDto Request) : IRequest<AccountDto>;
