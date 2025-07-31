using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Provider.Entities;

namespace Poliedro.Eds.Application.Provider.Commands.CreateProvider;

public class CreateProviderCommandHandler(
    IProviderCreateService ProviderCreateService,
    IMapper mapper,
    IValidator<CreateProviderRequestDto> validator,
    IRedisService redisService
    ) : IRequestHandler<CreateProviderCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var result = await ProviderCreateService.CreateAsync(mapper.Map<ProviderEntity>(request.Request));
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.PROVIDER);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
}
