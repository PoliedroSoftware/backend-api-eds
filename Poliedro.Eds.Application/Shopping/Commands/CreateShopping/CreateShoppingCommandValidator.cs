using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
using Poliedro.Eds.Application.Shopping.Commands.UpdateShopping;
using Poliedro.Eds.Domain.Provider.DomainProvider;

namespace Poliedro.Eds.Application.Shopping.Shopping.CreateShopping;

public class CreateShoppingCommandValidator : AbstractValidator<CreateShoppingRequestDto>
{
    private readonly IProviderGetByIdService _providerService;

    public CreateShoppingCommandValidator(IRedisService redisService, IProviderGetByIdService providerService)
    {
        _providerService = providerService;

        RuleFor(x => x.Invoice)
            .MustAsync(async (dto, invoice, cancellationToken) =>
            {
                // Get the provider to check if it's "Otros"
                var providerResult = await _providerService.GetByIdAsync(dto.IdProvider);
                if (providerResult.IsSuccess && providerResult.Value != null)
                {
                    // If provider is "Otros" (case insensitive), invoice is optional
                    if (string.Equals(providerResult.Value.Name, "otros", StringComparison.OrdinalIgnoreCase))
                    {
                        return true; // Invoice is optional for "Otros" provider
                    }
                }
                // For other providers, invoice is required
                return !string.IsNullOrWhiteSpace(invoice);
            })
            .WithMessage(redisService.GetValueFromCacheAsync("InvoiceNotEmpty").GetAwaiter().GetResult())
            .MaximumLength(45).WithMessage(redisService.GetValueFromCacheAsync("InvoiceMaximumLength").GetAwaiter().GetResult())
            .When(x => !string.IsNullOrWhiteSpace(x.Invoice)); // Only validate length when invoice is provided

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
