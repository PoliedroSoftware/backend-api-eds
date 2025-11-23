using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Hose.Commands.CreateHose;
using Poliedro.Eds.Application.Hose.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.PosOfSale.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Domain.PointOfSale.Entities;

namespace Poliedro.Eds.Application.PosOfSale.Commands.CreatePosOfSale
{
    public class CreatePosOfSaleCommandHandler(
       IPosOfSaleCreate posOfSaleDomain,
       IPosOfSaleQueryService posOfSaleQueryService,
       IRedisService redisService,
       IMapper mapper,
       IValidator<CreateHoseRequestDto> validator
       ) : IRequestHandler<CreatePosOfSaleCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreatePosOfSaleCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var posOfSaleEntity = mapper.Map<PosOfSaleEntity>(request.Request);

            var dispenserId = request.Request.IdDispensers;

            var posOfSaleErrorLimit = await posOfSaleQueryService.GetHoseLimitAsync(dispenserId);
            if (posOfSaleErrorLimit == null)
                return PosOfSaleErrorBuilder.PosOfSaleCreationException();

            var hoseCount = await posOfSaleQueryService.GetCurrentHoseCountAsync(dispenserId);
            if (hoseCount >= posOfSaleErrorLimit)
                return PosOfSaleErrorBuilder.PosOfSaleLimitErrorException();


            var result = await posOfSaleDomain.CreateAsync(posOfSaleEntity);
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.POS_OF_SALE);
            return result.IsSuccess ? result.Value! : result.Error!;

        }
    }
}
