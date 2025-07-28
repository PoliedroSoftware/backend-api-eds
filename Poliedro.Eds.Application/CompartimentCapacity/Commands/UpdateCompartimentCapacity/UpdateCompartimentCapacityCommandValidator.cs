using FluentValidation;
using Poliedro.Eds.Application.Court.Commands.CreateCourt;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.CompartimentCapacity.Commands.UpdateCompartimentCapacity;

public class UpdateCompartimentCapacityCommandValidator : AbstractValidator<UpdateCompartimentCapacityCommand>
{
    public UpdateCompartimentCapacityCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.IdCompartiment)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdCompartimentNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdCompartimentNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.IdCapacity)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdCapacityNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdCapacityNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Default)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("DefaultNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("DefaultNotEmpty").GetAwaiter().GetResult());
    }
}