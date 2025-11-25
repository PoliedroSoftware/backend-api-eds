using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.DomainPosOfSaleDetails;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Commands.CreatePosOfSale
{
    public class CreatePosOfSaleCommandHandler(
       IPosOfSaleDetailsCreateService posOfSaleDetailsDomain,
       IRedisService redisService,
       IMapper mapper,
       IValidator<CreatePosOfSaleDetailsRequestDto> validator
       ) : IRequestHandler<CreatePosOfSaleDetailsCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreatePosOfSaleDetailsCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var posOfSaleDetailsEntity = mapper.Map<PosOfSaleDetailsEntity>(request.Request);

            var result = await posOfSaleDetailsDomain.CreateAsync(posOfSaleDetailsEntity);
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.POS_OF_SALE_DETAILS);
            return result.IsSuccess ? result.Value! : result.Error!;

        }
    }
}
