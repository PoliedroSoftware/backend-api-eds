using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Expenditures.Commands.UpdateExpenditures;

public class UpdateExpendituresCommandValidator : AbstractValidator<UpdateExpendituresCommand>
{
    public UpdateExpendituresCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.Description)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("DescriptionNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("DescriptionNotEmpty").GetAwaiter().GetResult());
    }
}
