using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Compartiment.Commands.UpdateCompartiment;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;

namespace Poliedro.Eds.Application.Compartiment.UpdateCompartiment
{
    public class UpdateCompartimentCommandHandler(
        ICompartimentUpdateService compartimentDomainCompartiment,
        IMapper mapper,
        IValidator<UpdateCompartimentCommand> validator
        ) : IRequestHandler<UpdateCompartimentCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateCompartimentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var compartimentEntity = mapper.Map<CompartimentEntity>(request);
            var result = await compartimentDomainCompartiment.UpdateAsync(compartimentEntity);

            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}
