using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Hose.Commands.UpdateHose
{
    public class UpdateHoseCommandValidator : AbstractValidator<UpdateHoseCommand>
    {
        public UpdateHoseCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.Number)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NumberNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NumberNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.IdDispensers)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdDispensersNotNull").GetAwaiter().GetResult())
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdDispensersGreaterThan").GetAwaiter().GetResult());

            RuleFor(x => x.AccumulatedGallons)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("AccumulatedGallonsNotNull").GetAwaiter().GetResult())
                .GreaterThan(0.0).WithMessage(redisService.GetValueFromCacheAsync("AccumulatedGallonsGreaterThan").GetAwaiter().GetResult());

            RuleFor(x => x.AccumulatedAmount)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("AccumulatedAmountNotNull").GetAwaiter().GetResult())
                .GreaterThan(0.0).WithMessage(redisService.GetValueFromCacheAsync("AccumulatedAmountGreaterThan").GetAwaiter().GetResult());

            RuleFor(x => x.IdProductType)
               .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdProductTypeNotNull").GetAwaiter().GetResult())
               .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdProductTypeGreaterThan").GetAwaiter().GetResult());
        }
    }
}
