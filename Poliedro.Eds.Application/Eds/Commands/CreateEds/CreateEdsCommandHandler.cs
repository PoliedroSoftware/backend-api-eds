using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Application.Eds.Commands.CreateEds;

public class CreateEdsCommandHandler(
    IEdsCreateService EdsCreateService,
    IMapper mapper,
    IValidator<CreateEdsRequestDto> validator,
    IRedisService redisService
    ) : IRequestHandler<CreateEdsCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateEdsCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var result = await EdsCreateService.CreateAsync(mapper.Map<EdsEntity>(request.Request));
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.EDS);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
}
