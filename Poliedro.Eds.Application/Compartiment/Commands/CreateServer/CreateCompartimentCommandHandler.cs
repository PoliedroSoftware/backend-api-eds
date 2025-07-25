using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;
using System.Net;

namespace Poliedro.Eds.Application.Compartiment.Commands.CreateCompartiment
{
    public class CreateCompartimentCommandHandler(
        ICompartimentCreateService compartimentDomainService,
        IMapper mapper,
        IValidator<CreateCompartimentRequestDto> validator
        ) : IRequestHandler<CreateCompartimentCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateCompartimentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var compartimentEntity = mapper.Map<CompartimentEntity>(request.Request);
            var result = await compartimentDomainService.CreateAsync(compartimentEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}




