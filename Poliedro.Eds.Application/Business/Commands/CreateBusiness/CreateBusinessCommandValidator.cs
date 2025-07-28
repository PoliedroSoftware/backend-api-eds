using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.Business.Commands.CreateBusiness;

public class CreateBusinessCommandValidator : AbstractValidator<CreateBusinessRequestDto>
{
    public CreateBusinessCommandValidator(IRedisService redisService)
    {

        RuleFor(x => x.Name)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NameNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NameNotEmpty").GetAwaiter().GetResult())
            .NotEqual("string").WithMessage(redisService.GetValueFromCacheAsync("NameNotEqual").GetAwaiter().GetResult());

    }
}

