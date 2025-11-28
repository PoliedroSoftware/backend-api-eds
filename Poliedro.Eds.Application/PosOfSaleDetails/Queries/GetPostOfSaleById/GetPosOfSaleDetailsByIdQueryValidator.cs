using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Queries.GetPostOfSaleById
{
    public class GetPosOfSaleDetailsByIdQueryValidator : AbstractValidator<GetPosOfSaleDetailsByIdQuery>
    {
        public GetPosOfSaleDetailsByIdQueryValidator(IRedisService redisService)
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdNotNull").GetAwaiter().GetResult())
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdGreaterThan").GetAwaiter().GetResult());
        }
    }
}
