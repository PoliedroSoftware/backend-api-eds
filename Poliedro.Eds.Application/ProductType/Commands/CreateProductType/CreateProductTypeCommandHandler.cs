using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductType.DomainProductType;
using Poliedro.Eds.Domain.ProductType.Entities;
using System.Net;

namespace Poliedro.Eds.Application.ProductType.Commands.CreateProductType
{
    public class CreateProductTypeCommandHandler(
        IProductTypeCreateProductType productTypeDomainProductType,
        IMapper mapper,
        IValidator<CreateProductTypeRequestDto> validator,
        IRedisService redisService
        ) : IRequestHandler<CreateProductTypeCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateProductTypeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var result = await productTypeDomainProductType.CreateAsync(mapper.Map<ProductTypeEntity>(request.Request));
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.PRODUCT_TYPE);
            return result.IsSuccess ? result.Value! : result.Error!;
        }
    }
}




