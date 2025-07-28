using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.EdsTank.Commands.UpdateEdsTank;

public class UpdateEdsTankCommandValidator : AbstractValidator<UpdateEdsTankCommand>
{
    public UpdateEdsTankCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.IdEds)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdEdsNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdEdsNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.IdTank)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdTankNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdTankNotEmpty").GetAwaiter().GetResult());
    }
}