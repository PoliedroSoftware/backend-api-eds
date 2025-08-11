using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;
using Poliedro.Eds.Domain.HoseHistory.Entities;

namespace Poliedro.Eds.Application.HoseHistory.Commands.CreateHoseHistory
{
    public class CreateHoseHistoryCommandHandler(
        IHoseHistoryCreateHoseHistory hosehistoryDomainHoseHistory,
        IMapper mapper,
        IValidator<CreateHoseHistoryRequestDto> validator,
        IRedisService redisService
        ) : IRequestHandler<CreateHoseHistoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateHoseHistoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            // Round accumulated gallons to 2 decimal places for precision
            var requestWithRoundedGallons = request.Request with 
            { 
                AccumulatedGallons = Math.Round(request.Request.AccumulatedGallons, 2) 
            };

            var result = await hosehistoryDomainHoseHistory.CreateAsync(mapper.Map<HoseHistoryEntity>(requestWithRoundedGallons));
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.HOSE_HISTORY);
            return result.IsSuccess ? result.Value! : result.Error!;
        }
    }
}



