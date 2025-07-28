using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Capacity.Commands.UpdateCapacity;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;
using System.Net;

namespace Poliedro.Eds.Application.CompartimentCapacity.Commands.UpdateCompartimentCapacity;

public class UpdateCompartimentCapacityCommandHandler(
    ICompartimentCapacityUpdateService CompartimentCapacityDomainCompartimentCapacity,
    IMapper mapper,
    IValidator<UpdateCompartimentCapacityCommand> validator
    ) : IRequestHandler<UpdateCompartimentCapacityCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdateCompartimentCapacityCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var CompartimentCapacityEntity = mapper.Map<CompartimentCapacityEntity>(request);
        var result = await CompartimentCapacityDomainCompartimentCapacity.UpdateAsync(CompartimentCapacityEntity);

        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}