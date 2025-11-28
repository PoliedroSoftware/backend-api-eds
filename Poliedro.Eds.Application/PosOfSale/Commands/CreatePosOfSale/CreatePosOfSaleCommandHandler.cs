using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PointOfSale.Entities;
using Poliedro.Eds.Domain.PosOfSale.DomainPosOfSale;

namespace Poliedro.Eds.Application.PosOfSale.Commands.CreatePosOfSale
{
    public class CreatePosOfSaleCommandHandler(
       IPosOfSaleCreateService posOfSaleDomain,
       IRedisService redisService,
       IMapper mapper,
       IValidator<CreatePosOfSaleRequestDto> validator
       ) : IRequestHandler<CreatePosOfSaleCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreatePosOfSaleCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var posOfSaleEntity = mapper.Map<PosOfSaleEntity>(request.Request);

            var result = await posOfSaleDomain.CreateAsync(posOfSaleEntity);
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.POS_OF_SALE);
            return result.IsSuccess ? result.Value! : result.Error!;

        }
    }
}
