using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Poliedro.Eds.Application.StrongBox.Validation;
using Poliedro.Eds.Domain.StrongBox.Exceptions;
using Poliedro.Eds.Domain.StrongBox.Services;

namespace Poliedro.Eds.Application.StrongBox.Commands
{
    public class StrongBoxCreateCommandHandler(
        IStrongBoxService strongBoxService,
        IMapper mapper,
        StrongBoxCreateValidator validator) : IRequestHandler<StrongBoxCreateCommand, Unit>
    {
        async Task<Unit> IRequestHandler<StrongBoxCreateCommand, Unit>.Handle(StrongBoxCreateCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request.Request, cancellationToken);

            try
            {

                var created = await strongBoxService.CreateAsync(
                    request.Request.DateTime,
                    request.Request.IdCorte,
                    request.Request.Type,
                    request.Request.Ammount,
                    request.Request.Note,
                    cancellationToken);
            }
            catch (StrongBoxDomainException ex)
            {
                var failures = new List<ValidationFailure>
                {
                    new ValidationFailure("Ammount", ex.Message)
                };
                throw new ValidationException(failures);
            }

            return Unit.Value;
        }
    }
}
