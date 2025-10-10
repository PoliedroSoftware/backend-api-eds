using FluentValidation;
using FluentValidation;
using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
using Poliedro.Eds.Application.Shopping.Commands.UpdateShopping;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Provider.Entities;
using Newtonsoft.Json;

namespace Poliedro.Eds.Application.Shopping.Shopping.CreateShopping;

public class CreateShoppingCommandValidator : AbstractValidator<CreateShoppingRequestDto>
{
    private readonly IRedisService _redisService;
    private readonly IProviderGetAllService _providerGetAllService;

    public CreateShoppingCommandValidator(IRedisService redisService, IProviderGetAllService providerGetAllService)
    {
        _redisService = redisService;
        _providerGetAllService = providerGetAllService;

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
                        idProviderOtros = otrosProvider.IdProvider;
                        await _redisService.SetCacheAsync("IdProviderOtros", idProviderOtros.ToString(), TimeSpan.FromDays(1));
                    }
                }
                return x.IdProvider != idProviderOtros;
            })
            .WithMessage(_redisService.GetValueFromCacheAsync("InvoiceNotEmpty").GetAwaiter().GetResult())
            .MaximumLength(100).WithMessage(_redisService.GetValueFromCacheAsync("InvoiceMaximumLength").GetAwaiter().GetResult());

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
