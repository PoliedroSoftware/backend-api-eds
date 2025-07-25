using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Provider.Commands.UpdateProvider;

public class UpdateProviderCommandValidator : AbstractValidator<UpdateProviderCommand>
{
    public UpdateProviderCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.IdProvider).NotNull().GreaterThan(0)
             .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdProviderGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.Name)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NameNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NameNotEmpty").GetAwaiter().GetResult());
    }
}