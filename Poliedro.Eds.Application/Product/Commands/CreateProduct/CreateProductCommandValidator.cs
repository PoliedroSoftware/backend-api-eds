using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Product.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductRequestDto>
{
    public CreateProductCommandValidator(IRedisService redisService)
    {

        RuleFor(x => x.Name)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NameNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NameNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.IdProductType)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdProductTypeNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdProductTypeNotEmpty").GetAwaiter().GetResult());

<<<<<<< HEAD
        RuleFor(x => x.SellPrice)
=======
        RuleFor(x => x.Price)
>>>>>>> New-service-StrongBox
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("PriceNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("PriceNotEmpty").GetAwaiter().GetResult());
    }


}
