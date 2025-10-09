using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Account.Dtos;
using Poliedro.Eds.Domain.Account.Entities;
using Poliedro.Eds.Domain.Account.Services;

namespace Poliedro.Eds.Application.Account.Commands.CreateAccount;

public class CreateAccountCommandHandler(
    IAccountCreateService accountCreateService,
    IMapper mapper) : IRequestHandler<CreateAccountCommand, AccountDto>
{
    public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<AccountEntity>(request.Request);
        await accountCreateService.CreateAsync(entity, cancellationToken);
        
        return mapper.Map<AccountDto>(entity);
    }
}
