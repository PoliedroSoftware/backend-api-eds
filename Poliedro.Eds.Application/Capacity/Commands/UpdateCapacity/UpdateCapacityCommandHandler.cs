using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Capacity.Commands.UpdateCapacity;

public class UpdateCapacityCommandHandler(
    ICapacityUpdateService CapacityUpdateService,
    IMapper mapper,
    IValidator<UpdateCapacityCommand> validator
    ) : IRequestHandler<UpdateCapacityCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdateCapacityCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var CapacityEntity = mapper.Map<CapacityEntity>(request);
        var result = await CapacityUpdateService.UpdateAsync(CapacityEntity);

        if (!result.IsSuccess)
            return result.Error!;
        return result.Value!;
    }
}
