using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Poliedro.Eds.Application.Bank.Dtos;
using Poliedro.Eds.Application.Bank.Validation;
using Poliedro.Eds.Domain.Bank.Entities;
using Poliedro.Eds.Domain.Bank.Exceptions;
using Poliedro.Eds.Domain.Bank.Services;

namespace Poliedro.Eds.Application.Bank.Commands;

public class BankCreateCommandHandler(
    IBankService bankService,
    IMapper mapper,
    BankCreateValidator validator) : IRequestHandler<BankCreateCommand, BankDto>
{
    public async Task<BankDto> Handle(BankCreateCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Request, cancellationToken);

        try
        {
            var created = await bankService.CreateAsync(
                idAccount: request.Request.IdAccount,
                idEds: request.Request.IdEds,
                idCourt: request.Request.IdCourt,
                moviment: request.Request.Moviment?.Trim().ToUpperInvariant() ?? string.Empty,
                ammount: request.Request.Ammount,
                note: request.Request.Note,
                cancellationToken);
            
            return mapper.Map<BankDto>(created);
        }
        catch (BankDomainException ex)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("BankDomain", ex.Message)
            });
        }
    }
}
