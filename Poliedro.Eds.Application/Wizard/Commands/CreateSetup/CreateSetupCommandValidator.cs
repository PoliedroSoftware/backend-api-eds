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
        RuleFor(x => x.Bussiness)
        .NotNull()
        .WithMessage(redisService.GetValueFromCacheAsync("BussinessNotNull").GetAwaiter().GetResult());

        RuleFor(x => x.EDS)
        .NotNull()
        .WithMessage(redisService.GetValueFromCacheAsync("EDSNotNull").GetAwaiter().GetResult())
        .Must(list => list != null && list.Any())
        .WithMessage(redisService.GetValueFromCacheAsync("EDSMustHaveElements").GetAwaiter().GetResult());

        RuleFor(x => x.Islands)
        .NotNull()
        .WithMessage(redisService.GetValueFromCacheAsync("IslandsNotNull").GetAwaiter().GetResult())
        .Must(list => list != null && list.Any())
        .WithMessage(redisService.GetValueFromCacheAsync("IslandsMustHaveElements").GetAwaiter().GetResult());

        RuleFor(x => x.Tanks)
        .NotNull()
        .WithMessage(redisService.GetValueFromCacheAsync("TanksNotNull").GetAwaiter().GetResult())
        .Must(list => list != null && list.Any())
        .WithMessage(redisService.GetValueFromCacheAsync("TanksMustHaveElements").GetAwaiter().GetResult());

        RuleFor(x => x.Compartiments)
        .NotNull()
        .WithMessage(redisService.GetValueFromCacheAsync("CompartimentsNotNull").GetAwaiter().GetResult())
        .Must(list => list != null && list.Any())
        .WithMessage(redisService.GetValueFromCacheAsync("CompartimentsMustHaveElements").GetAwaiter().GetResult());

        RuleFor(x => x.Dispensers)
        .NotNull()
        .WithMessage(redisService.GetValueFromCacheAsync("DispensersNotNull").GetAwaiter().GetResult())
        .Must(list => list != null && list.Any())
        .WithMessage(redisService.GetValueFromCacheAsync("DispensersMustHaveElements").GetAwaiter().GetResult());

        RuleFor(x => x.Hoses)
        .NotNull()
        .WithMessage(redisService.GetValueFromCacheAsync("HosesNotNull").GetAwaiter().GetResult())
        .Must(list => list != null && list.Any())
        .WithMessage(redisService.GetValueFromCacheAsync("HosesMustHaveElements").GetAwaiter().GetResult());

        RuleFor(x => x.Products)
        .NotNull()
        .WithMessage(redisService.GetValueFromCacheAsync("ProductsNotNull").GetAwaiter().GetResult())
        .Must(list => list != null && list.Any())
        .WithMessage(redisService.GetValueFromCacheAsync("ProductsMustHaveElements").GetAwaiter().GetResult());

        RuleFor(x => x.Islanders)
        .NotNull()
        .WithMessage(redisService.GetValueFromCacheAsync("IslandersNotNull").GetAwaiter().GetResult())
        .Must(list => list != null && list.Any())
        .WithMessage(redisService.GetValueFromCacheAsync("IslandersMustHaveElements").GetAwaiter().GetResult());

        RuleFor(x => x.Providers)
        .NotNull()
        .WithMessage(redisService.GetValueFromCacheAsync("ProvidersNotNull").GetAwaiter().GetResult())
        .Must(list => list != null && list.Any())
        .WithMessage(redisService.GetValueFromCacheAsync("ProvidersMustHaveElements").GetAwaiter().GetResult());
    }
}
