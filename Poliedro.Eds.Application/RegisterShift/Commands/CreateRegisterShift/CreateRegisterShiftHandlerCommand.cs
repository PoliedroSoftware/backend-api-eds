using System;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.RegisterShift.Validations;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.RegisterShift.DomainService;
using Poliedro.Eds.Domain.RegisterShift.Entities;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Application.RegisterShift.Dtos;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Common.Constants;

namespace Poliedro.Eds.Application.RegisterShift.Commands.CreateRegisterShift;

public class CreateRegisterShiftHandlerCommand(
    IRegisterShiftCreateService registerShiftCreateService,
    IMapper mapper,
    IRedisService redisService,
    RegisterShiftCreateValidator validator)
    : IRequestHandler<CreateRegisterShiftCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateRegisterShiftCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.Request, cancellationToken);

        var entity = mapper.Map<RegisterShiftEntity>(request.Request);
        var result = await registerShiftCreateService.CreateAsync(entity);
        // No lanzar excepciones aquí; devolver el Result para que el controller lo mapee correctamente
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.REGISTER_SHIFT);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
}

