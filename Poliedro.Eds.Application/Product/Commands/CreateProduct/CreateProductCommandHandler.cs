using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Product.DomainProduct;
using Poliedro.Eds.Domain.Product.Entities;

namespace Poliedro.Eds.Application.Product.Commands.CreateProduct;

public class CreateProductCommandHandler(
    IProductCreateProduct ProductDomainProduct,
    IMapper mapper,
        IValidator<CreateProductRequestDto> validator,
        IRedisService redisService
    ) : IRequestHandler<CreateProductCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var entity = mapper.Map<ProductEntity>(request.Request);
       
        var result = await ProductDomainProduct.CreateAsync(entity);
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.PRODUCT);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
}
