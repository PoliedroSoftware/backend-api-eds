using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.TypeOfCollection.Commands.CreateTypeOfCollection;

public class CreateTypeOfCollectionCommandValidator : AbstractValidator<CreateTypeOfCollectionRequestDto>
{
    public CreateTypeOfCollectionCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.Description)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("DescriptionNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("DescriptionNotEmpty").GetAwaiter().GetResult());
    }


}
