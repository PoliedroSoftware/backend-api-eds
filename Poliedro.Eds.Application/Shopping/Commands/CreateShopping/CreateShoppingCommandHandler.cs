using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Product.Services;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Inventory.Entities;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Domain.Shopping.Entities;

namespace Poliedro.Eds.Application.Shopping.Commands.CreateShopping;

public class CreateShoppingCommandHandler(
    IShoppingCreateShopping shoppingDomainService,
    IMapper mapper,
    IValidator<CreateShoppingRequestDto> validator,
    IProductPriceUpdateService productPriceUpdateService,
    IRedisService redisService
) : IRequestHandler<CreateShoppingCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateShoppingCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var shoppingEntity = mapper.Map<ShoppingEntity>(request.Request);


        if (request.Request.SellPriceProducts is { } sellPriceProducts && sellPriceProducts.Any())
        {
            var products = mapper.Map<IEnumerable<ProductEntity>>(sellPriceProducts);
            var priceUpdateResult = await productPriceUpdateService.UpdatePricesAsync(products);
            if (!priceUpdateResult.IsSuccess)
                return priceUpdateResult.Error!;
        }

        shoppingEntity.ShoppingInventory = new InventoryEntity
        {
            Date = DateOnly.FromDateTime(shoppingEntity.Date),
            ReferenceType = "shopping",
        };

        var result = await shoppingDomainService.CreateAsync(shoppingEntity);

        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService,KeyRedisConstants.SHOPPING);

        return result.IsSuccess ? result.Value! : result.Error!;
    }
}
