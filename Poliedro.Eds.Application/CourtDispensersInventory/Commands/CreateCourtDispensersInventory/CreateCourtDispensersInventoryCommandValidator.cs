using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Commands.CreateCourtDispensersInventory;

public class CreateCourtDispensersInventoryCommandValidator : AbstractValidator<CreateCourtDispensersInventoryRequestDto>
{
    public CreateCourtDispensersInventoryCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.IdCourtdDispensers)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdCourtdDispensersNotNul").GetAwaiter().GetResult())
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdCourtdDispensersNotNul").GetAwaiter().GetResult());

        RuleFor(x => x.IdInventory)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdInventoryNotNull").GetAwaiter().GetResult())
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdInventoryGreaterThan").GetAwaiter().GetResult());
    }
}
