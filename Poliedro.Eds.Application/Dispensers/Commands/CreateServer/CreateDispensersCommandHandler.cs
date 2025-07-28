using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Dispensers.DomainDispensers;
using Poliedro.Eds.Domain.Dispensers.Entities;
using System.Net;

namespace Poliedro.Eds.Application.Dispensers.Commands.CreateDispensers
{
    public class CreateDispensersCommandHandler(
        IDispensersCreateDispensers dispensersDomainService,
        IMapper mapper,
        IValidator<CreateDispensersRequestDto> validator,
        IRedisService redisService
        ) : IRequestHandler<CreateDispensersCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateDispensersCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var result = await dispensersDomainService.CreateAsync(mapper.Map<DispensersEntity>(request.Request));
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.DISPENSERS);
            return result.IsSuccess ? result.Value! : result.Error!;
        }
    }
}




