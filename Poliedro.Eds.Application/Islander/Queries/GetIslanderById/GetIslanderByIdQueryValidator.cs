using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Islander.Queries.GetIslanderById
{
    public class GetIslanderyIdQueryValidator : AbstractValidator<GetIslanderByIdQuery>
    {
        public GetIslanderyIdQueryValidator(IRedisService redisService)
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdNotNull").GetAwaiter().GetResult())
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdGreaterThan").GetAwaiter().GetResult());
        }
    }
}
