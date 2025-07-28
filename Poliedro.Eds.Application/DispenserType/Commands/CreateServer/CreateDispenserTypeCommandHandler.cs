using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.DispenserType.DomainDispenserType;
using Poliedro.Eds.Domain.DispenserType.Entities;

namespace Poliedro.Eds.Application.DispenserType.Commands.CreateDispenserType

{
    public class CreateDispenserTypeCommandHandler(
        IDispenserTypeCreateDispenserType dispenserTypeDomainService,
        IMapper mapper,
        IValidator<CreateDispenserTypeRequestDto> validator
        ) : IRequestHandler<CreateDispenserTypeCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateDispenserTypeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var dispenserTypeEntity = mapper.Map<DispenserTypeEntity>(request.Request);
            var result = await dispenserTypeDomainService.CreateAsync(dispenserTypeEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}



