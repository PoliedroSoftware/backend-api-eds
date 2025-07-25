using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Hose.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;
using System.Net;

namespace Poliedro.Eds.Application.Hose.Commands.CreateHose
{
    public class CreateHoseCommandHandler(
        IHoseCreateHose hoseDomainHose,
        IHoseQueryService hoseQueryService,
        IRedisService redisService,
        IMapper mapper,
        IValidator<CreateHoseRequestDto> validator
        ):  IRequestHandler<CreateHoseCommand, Result<VoidResult, Error>>
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


            await redisService.RemoveByPrefixAsync("hose:");

            var result = await hoseDomainHose.CreateAsync(hoseEntity);

            if (!result)
                return HoseErrorBuilder.HoseCreationException();

            return VoidResult.Instance;
        }
    }
}



