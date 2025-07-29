using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Dispensers.DomainDispensers;
using Poliedro.Eds.Domain.Dispensers.Entities;

namespace Poliedro.Eds.Application.Dispensers.Commands.UpdateDispensers
{
    public class UpdateDispensersCommandHandler(
        IDispensersUpdateDispensers dispensersDomainDispensers,
        IMapper mapper,
        IValidator<UpdateDispensersCommand> validator
        ) : IRequestHandler<UpdateDispensersCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateDispensersCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var dispensersEntity = mapper.Map<DispensersEntity>(request);
            var result = await dispensersDomainDispensers.UpdateAsync(dispensersEntity);

            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}
