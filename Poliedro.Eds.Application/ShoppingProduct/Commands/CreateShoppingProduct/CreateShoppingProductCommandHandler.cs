using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using System.Net;

namespace Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
public class CreateShoppingProductCommandHandler(
        IShoppingProductCreateShoppingProduct shoppingProductDomainService,
        IMapper mapper,
        IValidator<CreateShoppingProductRequestDto> validator,
        IRedisService redisService
        ) : IRequestHandler<CreateShoppingProductCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateShoppingProductCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var result = await shoppingProductDomainService.CreateAsync(mapper.Map<ShoppingProductEntity>(request.Request));
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.SHOPPING_PRODUCT);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
    }





