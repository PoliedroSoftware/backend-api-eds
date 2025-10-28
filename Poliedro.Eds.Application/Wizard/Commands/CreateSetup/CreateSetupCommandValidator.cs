using System.Net;
using FluentValidation;
using Poliedro.Eds.Application.Eds.Queries.GetEdsById;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.Wizard.Commands.CreateSetup;

namespace Poliedro.Eds.Application.Eds.Commands.CreateEds;

public class CreateSetupCommandValidator : AbstractValidator<CreateSetupRequestDto>
{
    public CreateSetupCommandValidator(IRedisService redisService)
    {
        RuleFor(x
            => x.Bussiness)
        .NotNull()
        .WithMessage(redisService.GetValueFromCacheAsync("BussinessNotNull").GetAwaiter().GetResult() ?? "El campo 'Bussiness' no puede ser nulo.");

        RuleFor(x => x.EDS)
            .NotNull()
            .WithMessage(redisService.GetValueFromCacheAsync("EDSNotNull").GetAwaiter().GetResult()
                ?? "El campo 'EDS' no puede ser nulo.")
            .Must(list => list != null && list.Any())
            .WithMessage(redisService.GetValueFromCacheAsync("EDSMustHaveElements").GetAwaiter().GetResult()
                ?? "Debe existir al menos un elemento en 'EDS'.");

        RuleFor(x => x.Islands)
            .NotNull()
            .WithMessage(redisService.GetValueFromCacheAsync("IslandsNotNull").GetAwaiter().GetResult()
                ?? "El campo 'Islands' no puede ser nulo.")
            .Must(list => list != null && list.Any())
            .WithMessage(redisService.GetValueFromCacheAsync("IslandsMustHaveElements").GetAwaiter().GetResult()
                ?? "Debe existir al menos un elemento en 'Islands'.");

        RuleFor(x => x.Tanks)
            .NotNull()
            .WithMessage(redisService.GetValueFromCacheAsync("TanksNotNull").GetAwaiter().GetResult()
                ?? "El campo 'Tanks' no puede ser nulo.")
            .Must(list => list != null && list.Any())
            .WithMessage(redisService.GetValueFromCacheAsync("TanksMustHaveElements").GetAwaiter().GetResult()
                ?? "Debe existir al menos un elemento en 'Tanks'.");

        RuleFor(x => x.Compartiments)
            .NotNull()
            .WithMessage(redisService.GetValueFromCacheAsync("CompartimentsNotNull").GetAwaiter().GetResult()
                ?? "El campo 'Compartiments' no puede ser nulo.")
            .Must(list => list != null && list.Any())
            .WithMessage(redisService.GetValueFromCacheAsync("CompartimentsMustHaveElements").GetAwaiter().GetResult()
                ?? "Debe existir al menos un elemento en 'Compartiments'.");

        RuleFor(x => x.Dispensers)
            .NotNull()
            .WithMessage(redisService.GetValueFromCacheAsync("DispensersNotNull").GetAwaiter().GetResult()
                ?? "El campo 'Dispensers' no puede ser nulo.")
            .Must(list => list != null && list.Any())
            .WithMessage(redisService.GetValueFromCacheAsync("DispensersMustHaveElements").GetAwaiter().GetResult()
                ?? "Debe existir al menos un elemento en 'Dispensers'.");

        RuleFor(x => x.Hoses)
            .NotNull()
            .WithMessage(redisService.GetValueFromCacheAsync("HosesNotNull").GetAwaiter().GetResult()
                ?? "El campo 'Hoses' no puede ser nulo.")
            .Must(list => list != null && list.Any())
            .WithMessage(redisService.GetValueFromCacheAsync("HosesMustHaveElements").GetAwaiter().GetResult()
                ?? "Debe existir al menos un elemento en 'Hoses'.");

        RuleFor(x => x.Products)
            .NotNull()
            .WithMessage(redisService.GetValueFromCacheAsync("ProductsNotNull").GetAwaiter().GetResult()
                ?? "El campo 'Products' no puede ser nulo.")
            .Must(list => list != null && list.Any())
            .WithMessage(redisService.GetValueFromCacheAsync("ProductsMustHaveElements").GetAwaiter().GetResult()
                ?? "Debe existir al menos un elemento en 'Products'.");

        RuleFor(x => x.Islanders)
            .NotNull()
            .WithMessage(redisService.GetValueFromCacheAsync("IslandersNotNull").GetAwaiter().GetResult()
                ?? "El campo 'Islanders' no puede ser nulo.")
            .Must(list => list != null && list.Any())
            .WithMessage(redisService.GetValueFromCacheAsync("IslandersMustHaveElements").GetAwaiter().GetResult()
                ?? "Debe existir al menos un elemento en 'Islanders'.");

        RuleFor(x => x.Providers)
            .NotNull()
            .WithMessage(redisService.GetValueFromCacheAsync("ProvidersNotNull").GetAwaiter().GetResult()
                ?? "El campo 'Providers' no puede ser nulo.")
            .Must(list => list != null && list.Any())
            .WithMessage(redisService.GetValueFromCacheAsync("ProvidersMustHaveElements").GetAwaiter().GetResult()
                ?? "Debe existir al menos un elemento en 'Providers'.");

    }
}
