using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Capacity.Commands.CreateCapacity;

public class CreateCapacityCommandHandler(
    ICapacityCreateService CapacityCreateService,
    IMapper mapper,
    IValidator<CreateCapacityRequestDto> validator,
    IRedisService redisService) : IRequestHandler<CreateCapacityCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateCapacityCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var result = await CapacityCreateService.CreateAsync(mapper.Map<CapacityEntity>(request.Request));
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.CAPACITY);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
}