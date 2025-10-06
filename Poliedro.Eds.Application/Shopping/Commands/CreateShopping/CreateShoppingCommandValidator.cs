using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
using Poliedro.Eds.Application.Shopping.Commands.UpdateShopping;
using Poliedro.Eds.Domain; // Ahora PaginationParams está directamente en Poliedro.Eds.Domain
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Provider.Entities;

namespace Poliedro.Eds.Application.Shopping.Shopping.CreateShopping;

public class CreateShoppingCommandValidator : AbstractValidator<CreateShoppingRequestDto>
{
    public CreateShoppingCommandValidator(IRedisService redisService, IProviderGetAllService providerGetAllService)
    {
        var otherProvider = providerGetAllService.GetAllAsync(new PaginationParams()).GetAwaiter().GetResult().FirstOrDefault(p => p.Name.ToLower() == "otros");
        var otherProviderId = otherProvider?.IdProvider ?? 0;

        RuleFor(x => x.Invoice)
            .NotEmpty()
            .When(x => x.IdProvider != otherProviderId)
            .WithMessage(redisService.GetValueFromCacheAsync("InvoiceNotEmpty)").GetAwaiter().GetResult())
            .MaximumLength(100).WithMessage(redisService.GetValueFromCacheAsync("InvoiceMaximumLength)").GetAwaiter().GetResult());

        //RuleFor(x => x.Date)
        //    .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("DateNotEmpty").GetAwaiter().GetResult())
        //    .Must(date => date != default(DateTime)).WithMessage(redisService.GetValueFromCacheAsync("DateMust").GetAwaiter().GetResult());

        //RuleFor(x => x.IdProvider)
        //    .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdProviderGreaterThan").GetAwaiter().GetResult());

        //RuleFor(x => x.IdCategory)
        //    .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdCategoryGreaterThan").GetAwaiter().GetResult());

        //RuleFor(x => x.Amount)
        //    .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("AmountGreaterThan").GetAwaiter().GetResult());
    }
}
