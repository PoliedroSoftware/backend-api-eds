using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;
using System.Net;

namespace Poliedro.Eds.Application.Tank.Commands.CreateTank;

public class CreateTankCommandHandler(
    ITankCreateTank tankDomainTank,
    IMapper mapper,
    IValidator<CreateTankRequestDto> validator,
    IRedisService redisService
    ) : IRequestHandler<CreateTankCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateTankCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var result = await tankDomainTank.CreateAsync(mapper.Map<TankEntity>(request.Request));
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.TANK);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
}



