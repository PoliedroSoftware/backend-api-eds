using FluentValidation;
using Poliedro.Eds.Application.Hose.Queries.GetLastAccumulated;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Hose.Queries.GetHoseById
{
    public class GetLastAccumulatedQueryValidator : AbstractValidator<GetLastAccumulatedQuery>
    {
        public GetLastAccumulatedQueryValidator(IRedisService redisService)
        {
            RuleFor(x => x.idDispenser)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("idDispenserNotNull").GetAwaiter().GetResult())
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("idDispenserGreaterThan").GetAwaiter().GetResult());
            RuleFor(x => x.idHose)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("idHoseNotNull").GetAwaiter().GetResult())
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("idHoseGreaterThan").GetAwaiter().GetResult());
        }
    }
}
