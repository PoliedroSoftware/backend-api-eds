using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Application.StrongBox.Validation;
using Poliedro.Eds.Domain.StrongBox.Entities;
using Poliedro.Eds.Domain.StrongBox.Exceptions;
using Poliedro.Eds.Domain.StrongBox.Services;

namespace Poliedro.Eds.Application.StrongBox.Commands;

public class StrongBoxCreateCommandHandler(
    IStrongBoxService strongBoxService,
    IMapper mapper,
    StrongBoxCreateValidator validator) : IRequestHandler<StrongBoxCreateCommand, StrongBoxDto>
{
    public async Task<StrongBoxDto> Handle(StrongBoxCreateCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Request, cancellationToken);

        try
        {
            var created = await  strongBoxService.CreateAsync(
                idCorte: request.Request.IdCorte,
                type: request.Request.Type?.Trim().ToUpperInvariant() ?? string.Empty,
                ammount: request.Request.Ammount,
                note: request.Request.Note,
                cancellationToken);
            
            return mapper.Map<StrongBoxDto>(created);
        }
        catch (StrongBoxDomainException ex)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("StrongBoxDomain", ex.Message)
            });
        }
    }  
}
