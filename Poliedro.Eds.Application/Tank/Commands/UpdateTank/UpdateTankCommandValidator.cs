using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Tank.Commands.UpdateTank;

public class UpdateTankCommandValidator : AbstractValidator<UpdateTankCommand>
{
    public UpdateTankCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.Number)
        .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NumberNotNull").GetAwaiter().GetResult())
        .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NumberNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Compartment)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("CompartmentNotNull").GetAwaiter().GetResult())
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("CompartmentGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.Ability)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("AbilityNotNull").GetAwaiter().GetResult())
            .GreaterThan(0.0).WithMessage(redisService.GetValueFromCacheAsync("AbilityGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.Stock)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("StockNotNull").GetAwaiter().GetResult())
            .GreaterThan(0.0).WithMessage(redisService.GetValueFromCacheAsync("StockGreaterThan").GetAwaiter().GetResult());
    }
}
