using FluentValidation;
using Poliedro.Eds.Application.Capacity.Queries.GetCapacityById;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Capacity.Commands.CreateCapacity;

public class CreateCapacityCommandValidator : AbstractValidator<CreateCapacityRequestDto>
{
    public CreateCapacityCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.Code)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("CodeNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("CodeNotEmpty").GetAwaiter().GetResult())
            .NotEqual("string").WithMessage(redisService.GetValueFromCacheAsync("CodeNotEqual").GetAwaiter().GetResult());

        RuleFor(x => x.Height)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("HeightNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("HeightNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Gallon)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("GallonNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("GallonNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Liters)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("LitersNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("LitersNotEmpty").GetAwaiter().GetResult());

    }

    public class GetCapacityByIdCommandValidator : AbstractValidator<GetCapacityByIdQuery>
    {
        public GetCapacityByIdCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdGreaterThan").GetAwaiter().GetResult());
        }
    }
}
