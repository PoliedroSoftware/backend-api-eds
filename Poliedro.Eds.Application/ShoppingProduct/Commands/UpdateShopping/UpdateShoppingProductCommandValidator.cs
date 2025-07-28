using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.ShoppingProduct.Commands.UpdateShoppingProduct;
namespace Poliedro.Eds.Application.Shopping.Commands.UpdateShoppingProduct;

public class UpdateShoppingProductCommandValidator : AbstractValidator<UpdateShoppingProductCommand>
{
    public UpdateShoppingProductCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.IdShoppingProduct)
        .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("GreaterThanIdShoppingProduct").GetAwaiter().GetResult())
        .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdShoppingProductNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.IdShopping)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("GreaterThanIdShopping").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NotEmptyIdShopping").GetAwaiter().GetResult());

        RuleFor(x => x.IdProduct)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdProductGreaterThan").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdProductNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("QuantityGreaterThanOrEqualTo").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("QuantityNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("PriceGreaterThanOrEqualTo").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("PriceNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.IdCompartment)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdCompartmentGreaterThan").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdCompartmentNotEmpty").GetAwaiter().GetResult());
    }
}

