using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Hose.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;

namespace Poliedro.Eds.Application.Hose.Commands.CreateHose
{
    public class CreateHoseCommandHandler(
        IHoseCreateHose hoseDomainHose,
        IHoseQueryService hoseQueryService,
        IRedisService redisService,
        IMapper mapper,
        IValidator<CreateHoseRequestDto> validator
        ) : IRequestHandler<CreateHoseCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateHoseCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var hoseEntity = mapper.Map<HoseEntity>(request.Request);

            var dispenserId = request.Request.IdDispensers;

            var hoseLimit = await hoseQueryService.GetHoseLimitAsync(dispenserId);
            if (hoseLimit == null)
                return HoseErrorBuilder.HoseCreationException();

            var hoseCount = await hoseQueryService.GetCurrentHoseCountAsync(dispenserId);
            if (hoseCount >= hoseLimit)
                return HoseErrorBuilder.HoseLimitErrorException();


            var result = await hoseDomainHose.CreateAsync(hoseEntity);
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.HOSE);
            return result.IsSuccess ? result.Value! : result.Error!;

        }
    }
}



