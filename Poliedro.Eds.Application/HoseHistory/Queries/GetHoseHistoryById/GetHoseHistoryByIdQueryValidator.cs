using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.HoseHistory.Queries.GetHoseHistoryById
{
    public class GetHoseHistoryIdQueryValidator : AbstractValidator<GetHoseHistoryByIdQuery>
    {
        public GetHoseHistoryIdQueryValidator(IRedisService redisService)
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdNotNull").GetAwaiter().GetResult())
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdGreaterThan").GetAwaiter().GetResult());
        }
    }
}
