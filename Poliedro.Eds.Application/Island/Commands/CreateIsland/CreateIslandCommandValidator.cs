using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Island.Commands.CreateIsland;

public class CreateIslandCommandValidator : AbstractValidator<CreateIslandRequestDto>
{
    public CreateIslandCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.Description)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NameNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NameNotEmpty").GetAwaiter().GetResult());
    }
}
