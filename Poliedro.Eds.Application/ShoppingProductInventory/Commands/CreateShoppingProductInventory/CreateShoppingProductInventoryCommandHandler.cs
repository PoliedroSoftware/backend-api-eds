using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Commands.CreateShoppingProductInventory
{
    public class CreateShoppingProductInventoryCommandHandler(
        IShoppingCreateShoppingProductInventory shoppingProductDomainService,
        IMapper mapper,
        IValidator<CreateShoppingProductInventoryRequestDto> validator,
        IRedisService redisService
        ) : IRequestHandler<CreateShoppingProductInventoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateShoppingProductInventoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var result = await shoppingProductDomainService.CreateAsync(mapper.Map<ShoppingProductInventoryEntity>(request.Request));
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.SHOPPING_PRODUCT_INVENTORY);
            return result.IsSuccess ? result.Value! : result.Error!;

        }
    }
}
