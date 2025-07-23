using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Islander.Commands.UpdateIslander
{
    public class UpdateIslanderCommandValidator : AbstractValidator<UpdateIslanderCommand>
    {
        public UpdateIslanderCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NameNotNull").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NameNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.IdEds).NotNull().GreaterThan(0)
                 .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdEdsGreaterThan").GetAwaiter().GetResult());
        }
    }
}
