using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
using Poliedro.Eds.Application.Shopping.Commands.UpdateShopping;

namespace Poliedro.Eds.Application.Shopping.Shopping.CreateShopping;

public class CreateShoppingCommandValidator : AbstractValidator<CreateShoppingRequestDto>
{
    public CreateShoppingCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.Invoice)
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("InvoiceNotEmpty)").GetAwaiter().GetResult())
            .MaximumLength(45).WithMessage(redisService.GetValueFromCacheAsync("InvoiceMaximumLength)").GetAwaiter().GetResult());

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("DateNotEmpty").GetAwaiter().GetResult())
            .Must(date => date != default(DateTime)).WithMessage(redisService.GetValueFromCacheAsync("DateMust").GetAwaiter().GetResult());

        RuleFor(x => x.IdProvider)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdProviderGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.IdCategory)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdCategoryGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("AmountGreaterThan").GetAwaiter().GetResult());
    }
}
