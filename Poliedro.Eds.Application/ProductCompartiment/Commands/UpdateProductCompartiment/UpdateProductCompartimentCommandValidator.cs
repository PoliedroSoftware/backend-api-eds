using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.ProductCompartiment.Commands.UpdateProductCompartiment
{
    public class UpdateProductCompartimentCommandValidator : AbstractValidator<UpdateProductCompartimentCommand>
    {
        public UpdateProductCompartimentCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.IdProduct)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdProductNotNull").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdProductNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.IdProductCompartiment)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdProductCompartimentNotNull").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdProductCompartimentNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.Stock)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("StockNotNull").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("StockNotEmpty").GetAwaiter().GetResult());
        }
    }
}
