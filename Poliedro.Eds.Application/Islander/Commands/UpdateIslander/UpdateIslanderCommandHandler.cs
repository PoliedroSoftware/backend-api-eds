using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Capacity.Commands.UpdateCapacity;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;
using System.Net;

namespace Poliedro.Eds.Application.Islander.Commands.UpdateIslander
{
    public class UpdateIslanderCommandHandler(
        IIslanderUpdateIslander islanderDomainIslander,
        IMapper mapper,
        IValidator<UpdateIslanderCommand> validator
    ) : IRequestHandler<UpdateIslanderCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateIslanderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var islanderEntity = mapper.Map<IslanderEntity>(request);
            var result = await islanderDomainIslander.UpdateAsync(islanderEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }
}