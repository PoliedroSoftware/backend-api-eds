using FluentValidation;
using Poliedro.Eds.Application.DispenserType.Commands.CreateDispenserType;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;


namespace Poliedro.Eds.Application.Server.DispenserType.CreateDispenserType;

public class CreateDispenserTypeCommandValidator : AbstractValidator<CreateDispenserTypeRequestDto>
{
    public CreateDispenserTypeCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("DescriptionNotEmpty").GetAwaiter().GetResult())
            .MaximumLength(50).WithMessage(redisService.GetValueFromCacheAsync("DescriptionMaximumLength").GetAwaiter().GetResult());
    }
}
