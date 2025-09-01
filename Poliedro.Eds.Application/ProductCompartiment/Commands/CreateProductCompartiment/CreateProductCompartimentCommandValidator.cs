using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.ProductCompartiment.Commands.CreateProductCompartiment;

public class CreateProductCompartimentCommandValidator : AbstractValidator<CreateProductCompartimentRequestDto>
{
    public CreateProductCompartimentCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.IdProduct)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdProductNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdProductNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.IdCompartiment)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdCompartimentNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdCompartimentNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Stock)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("StockNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("StockNotEmpty").GetAwaiter().GetResult());
    }
}
