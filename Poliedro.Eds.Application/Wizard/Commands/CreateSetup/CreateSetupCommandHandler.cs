using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Wizard.DomainSetup;
using Poliedro.Eds.Domain.Wizard.Entities;

namespace Poliedro.Eds.Application.Wizard.Commands.CreateSetup;

public class CreateSetupCommandHandler(
    ISetupCreateService setupCreateService,
    IMapper mapper,
    IValidator<CreateSetupRequestDto> validator,
    IRedisService redisService
    ) : IRequestHandler<CreateSetupCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateSetupCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var result = await setupCreateService.CreateAsync(mapper.Map<SetupEntity>(request.Request));
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.SETUP);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
}
