using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.DispenserType.DomainDispenserType;
using Poliedro.Eds.Domain.DispenserType.Entities;
using System.Net;

namespace Poliedro.Eds.Application.DispenserType.Commands.CreateDispenserType

{
    public class CreateDispenserTypeCommandHandler(
        IDispenserTypeCreateDispenserType dispenserTypeDomainService,
        IMapper mapper,
        IValidator<CreateDispenserTypeRequestDto> validator,
        IRedisService redisService
        ) : IRequestHandler<CreateDispenserTypeCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateDispenserTypeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var result = await dispenserTypeDomainService.CreateAsync(mapper.Map<DispenserTypeEntity>(request.Request));
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.DISPENSER_TYPE);
            return result.IsSuccess ? result.Value! : result.Error!;
        }
    }
}



