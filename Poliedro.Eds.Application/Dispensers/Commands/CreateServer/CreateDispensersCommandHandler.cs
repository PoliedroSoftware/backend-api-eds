using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Dispensers.DomainDispensers;
using Poliedro.Eds.Domain.Dispensers.Entities;

namespace Poliedro.Eds.Application.Dispensers.Commands.CreateDispensers
{
    public class CreateDispensersCommandHandler(
        IDispensersCreateDispensers dispensersDomainService,
        IMapper mapper,
        IValidator<CreateDispensersRequestDto> validator
        ) : IRequestHandler<CreateDispensersCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateDispensersCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var dispensersEntity = mapper.Map<DispensersEntity>(request.Request);
            var result = await dispensersDomainService.CreateAsync(dispensersEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}




