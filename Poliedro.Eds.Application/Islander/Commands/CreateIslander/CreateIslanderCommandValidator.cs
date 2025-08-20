using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Islander.Commands.CreateIslander;

public class CreateIslanderCommandValidator : AbstractValidator<CreateIslanderRequestDto>
{
    public CreateIslanderCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NameNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NameNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.IdEds)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdEdsNotNull").GetAwaiter().GetResult())
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdEdsNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Password)
           .NotNull().WithMessage(redisService.GetValueFromCacheAsync("PasswordNotNull").GetAwaiter().GetResult())
           .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("PasswordNotEmpty").GetAwaiter().GetResult());
    }
}
