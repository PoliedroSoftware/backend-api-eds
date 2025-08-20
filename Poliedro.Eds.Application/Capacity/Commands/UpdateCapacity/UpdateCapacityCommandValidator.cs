using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Capacity.Commands.UpdateCapacity;

public class UpdateCapacityCommandValidator : AbstractValidator<UpdateCapacityCommand>
{
    public UpdateCapacityCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.IdCapacity).NotNull().GreaterThan(0)
             .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdCapacityGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.Code)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("CodeNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("CodeNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Height)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("HeightNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("HeightNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Gallon)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("GallonNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("GallonNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Liters)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("LitersNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("LitersNotEmpty").GetAwaiter().GetResult());

    }
}
