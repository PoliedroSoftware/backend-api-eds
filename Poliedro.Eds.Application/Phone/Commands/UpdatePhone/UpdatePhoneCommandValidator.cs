using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.Phone.Commands.UpdatePhone;

public class UpdatePhoneCommandValidator : AbstractValidator<UpdatePhoneCommand>
{
    public UpdatePhoneCommandValidator(IRedisService redisService)
    {
        //RuleFor(x => x.IdPhone)
        //    .NotNull()
        //    .GreaterThan(0)
        //    .WithMessage(redisService.GetValueFromCacheAsync("IdPhoneGreaterThan").GetAwaiter().GetResult());

        //RuleFor(x => x.Number)
        //    .NotNull().WithMessage(redisService.GetValueFromCacheAsync("PhoneNumberNotNull").GetAwaiter().GetResult())
        //    .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("PhoneNumberNotEmpty").GetAwaiter().GetResult());
    }
}
