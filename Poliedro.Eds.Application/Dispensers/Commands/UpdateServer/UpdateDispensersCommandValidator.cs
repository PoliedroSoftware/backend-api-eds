using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
namespace Poliedro.Eds.Application.Dispensers.Commands.UpdateDispensers
{
    public class UpdateDispensersCommandValidator : AbstractValidator<UpdateDispensersCommand>
    {
        public UpdateDispensersCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdGreaterThan").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("CodeNotEmpty").GetAwaiter().GetResult())
                .MaximumLength(50).WithMessage(redisService.GetValueFromCacheAsync("CodeMaximumLength").GetAwaiter().GetResult());

            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("NumberGreaterThan").GetAwaiter().GetResult());

            RuleFor(x => x.DispenserTypeId)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("DispenserTypeIdGreaterThan").GetAwaiter().GetResult());

            RuleFor(x => x.HoseNumber)
                .GreaterThanOrEqualTo(1).WithMessage(redisService.GetValueFromCacheAsync("HoseNumberGreaterThanOrEqualTo").GetAwaiter().GetResult());

            RuleFor(x => x.EdsId)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("EdsIdGreaterThan").GetAwaiter().GetResult());

            RuleFor(x => x.IdIsland)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdIslandGreaterThan").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdIslandNotEmpty").GetAwaiter().GetResult());
        }
    }
}
