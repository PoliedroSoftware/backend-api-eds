using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Product.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NameNotNull").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NameNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.IdProductType)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdProductTypeNotNull").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdProductTypeNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.Price)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("PriceNotNull").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("PriceNotEmpty").GetAwaiter().GetResult());


        }
    }
}
