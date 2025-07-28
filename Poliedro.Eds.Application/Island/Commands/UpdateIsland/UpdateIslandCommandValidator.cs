using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Island.Commands.UpdateIsland
{
    public class UpdateIslandCommandValidator : AbstractValidator<UpdateIslandCommand>
    {
        public UpdateIslandCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.Description)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NameNotNull").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NameNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.IdIsland).NotNull().GreaterThan(0)
                 .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdEdsGreaterThan").GetAwaiter().GetResult());
        }
    }
}