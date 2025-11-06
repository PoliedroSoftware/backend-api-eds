using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Capacity.Dtos;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Capacity.Commands.CreateCapacity;

public class CreateCapacityCommandHandler(
    ICapacityCreateService CapacityCreateService,
    IMapper mapper,
    IValidator<CreateCapacityRequestDto> validator,
    IRedisService redisService) : IRequestHandler<CreateCapacityCommand, Result<CapacityDto, Error>>
{
    public async Task<Result<CapacityDto, Error>> Handle(CreateCapacityCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<CapacityDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var result = await CapacityCreateService.CreateAsync(mapper.Map<CapacityEntity>(request.Request));
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.CAPACITY);
        return result.IsSuccess ? mapper.Map<CapacityDto>(result.Value!) : result.Error!;
    }
}
