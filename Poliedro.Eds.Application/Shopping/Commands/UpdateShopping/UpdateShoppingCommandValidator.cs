using FluentValidation;
using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Provider.Entities;
using Newtonsoft.Json;

namespace Poliedro.Eds.Application.Shopping.Commands.UpdateShopping;

public class UpdateShoppingCommandValidator : AbstractValidator<UpdateShoppingCommand>
{
    private readonly IRedisService _redisService;
    private readonly IProviderGetAllService _providerGetAllService;

    public UpdateShoppingCommandValidator(IRedisService redisService, IProviderGetAllService providerGetAllService)
    {
        _redisService = redisService;
        _providerGetAllService = providerGetAllService;

        RuleFor(x => x.IdShopping)
            .GreaterThan(0).WithMessage(_redisService.GetValueFromCacheAsync("IdShoppingGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.Invoice)
            .NotEmpty().WhenAsync(async (x, cancellationToken) =>
            {
                int? idProviderOtros = null;
                var cachedId = await _redisService.GetValueFromCacheAsync("IdProviderOtros");
                if (!string.IsNullOrEmpty(cachedId))
                {
                    idProviderOtros = int.Parse(cachedId);
                }
                else
                {
                    var allProviders = await _providerGetAllService.GetAllAsync(new PaginationParams { PageNumber = 1, PageSize = 1000 });
                    var otrosProvider = allProviders.FirstOrDefault(p => p.Name.Equals("otros", StringComparison.OrdinalIgnoreCase));
                    if (otrosProvider != null)
                    {
                        await _redisService.SetCacheAsync("IdProviderOtros", otrosProvider.IdProvider.ToString(), TimeSpan.FromDays(1));
                        idProviderOtros = otrosProvider.IdProvider;
                    }
                }
                return x.IdProvider != idProviderOtros;
            })
            .WithMessage(_redisService.GetValueFromCacheAsync("InvoiceNotEmpty").GetAwaiter().GetResult())
            .MaximumLength(45).WithMessage(_redisService.GetValueFromCacheAsync("InvoiceMaximumLength").GetAwaiter().GetResult());

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

