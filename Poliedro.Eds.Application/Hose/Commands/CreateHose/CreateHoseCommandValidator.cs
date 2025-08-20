using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Hose.Commands.CreateHose;

public class CreateHoseCommandValidator : AbstractValidator<CreateHoseRequestDto>
{
    public CreateHoseCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.Number)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NumberNotNull").GetAwaiter().GetResult())
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("NumberGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.IdDispensers)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdDispensersNotNull").GetAwaiter().GetResult())
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdDispensersGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.IdProductType)
           .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdProductTypeNotNull").GetAwaiter().GetResult())
           .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdProductTypeGreaterThan").GetAwaiter().GetResult());
    }
}
