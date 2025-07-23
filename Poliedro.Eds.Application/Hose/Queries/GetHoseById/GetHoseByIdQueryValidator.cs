using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Hose.Queries.GetHoseById
{
    public class GetHoseIdQueryValidator : AbstractValidator<GetHoseByIdQuery>
    {
        public GetHoseIdQueryValidator(IRedisService redisService)
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdNotNull").GetAwaiter().GetResult())
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdGreaterThan").GetAwaiter().GetResult());
        }
    }
}
