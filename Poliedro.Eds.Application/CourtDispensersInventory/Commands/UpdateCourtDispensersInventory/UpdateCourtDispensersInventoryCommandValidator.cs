using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Commands.UpdateCourtDispensersInventory
{
    public class UpdateCourtDispensersInventoryCommandValidator : AbstractValidator<UpdateCourtDispensersInventoryCommand>
    {
        public UpdateCourtDispensersInventoryCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.IdCourtdDispensers)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdCourtdDispensersNotNull").GetAwaiter().GetResult())
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdCourtdDispensersGreaterThan").GetAwaiter().GetResult());

            RuleFor(x => x.IdInventory)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdInventoryNotNull").GetAwaiter().GetResult())
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdInventoryGreaterThan").GetAwaiter().GetResult());
        }
    }
}
