using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.DispenserType.Commands.UpdateDispenserType;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.DispenserType.DomainDispenserType;
using Poliedro.Eds.Domain.DispenserType.Entities;

namespace Poliedro.Eds.Application.DispenserType.Commands.UpdateDispneserType
{
    public class UpdateDispenserTypeCommandHandler(
        IDispenserTypeUpdateDispenserType dispenserTypeDomainDispenserType,
        IMapper mapper,
        IValidator<UpdateDispenserTypeCommand> validator
    ) : IRequestHandler<UpdateDispenserTypeCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateDispenserTypeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var dispenserTypeEntity = mapper.Map<DispenserTypeEntity>(request);
            var result = await dispenserTypeDomainDispenserType.UpdateAsync(dispenserTypeEntity);

            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}
