using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Business.DomaianServices.Create;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Business.Commands.CreateBusiness;

public class CreateBusinessCommandHandler(
    IBusinessCreateDomianService businessCreateDomianService,
    IMapper mapper,
    IValidator<CreateBusinessRequestDto> validator,
    IRedisService redisService) : IRequestHandler<CreateBusinessCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateBusinessCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed",
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)) ?? string.Empty,
                    HttpStatusCode.BadRequest));

        var result = await businessCreateDomianService.CreateAsync(mapper.Map<BusinessEntity>(request.Request));
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.BUSINESS);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
}
