using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Capacity.Commands.CreateCapacity;

public class CreateCapacityCommandHandler(
    ICapacityCreateService CapacityCreateService,
    IMapper mapper,
    IValidator<CreateCapacityRequestDto> validator) : IRequestHandler<CreateCapacityCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateCapacityCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var CapacityEntity = mapper.Map<CapacityEntity>(request.Request);
        var result = await CapacityCreateService.CreateAsync(CapacityEntity);
        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}