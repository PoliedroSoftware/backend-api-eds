using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Capacity.Queries.GetCapacityById;

public class GetCapacityIdQueryValidator : AbstractValidator<GetCapacityByIdQuery>
{
    public GetCapacityIdQueryValidator(IRedisService redisService)
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NotNull").GetAwaiter().GetResult())
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("GreaterThan").GetAwaiter().GetResult());
    }
}
