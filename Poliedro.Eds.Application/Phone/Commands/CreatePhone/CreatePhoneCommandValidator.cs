using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.Phone.Commands.CreatePhone;

public class CreatePhoneCommandValidator : AbstractValidator<CreatePhoneRequestDto>
{
    public CreatePhoneCommandValidator(IRedisService redisService)
    {
        //RuleFor(x => x.Number)
        //    .NotNull().WithMessage(redisService.GetValueFromCacheAsync("PhoneNumberNotNull").GetAwaiter().GetResult())
        //    .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("PhoneNumberNotEmpty").GetAwaiter().GetResult())
        //    .MaximumLength(13).WithMessage(redisService.GetValueFromCacheAsync("PhoneNumberMaxLength").GetAwaiter().GetResult());
    }
}
