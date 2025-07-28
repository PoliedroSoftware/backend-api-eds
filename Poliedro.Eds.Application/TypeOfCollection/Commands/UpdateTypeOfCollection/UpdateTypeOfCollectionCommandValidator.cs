using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.TypeOfCollection.Commands.UpdateTypeOfCollection;

public class UpdateTypeOfCollectionCommandValidator : AbstractValidator<UpdateTypeOfCollectionCommand>
{
    public UpdateTypeOfCollectionCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.Description)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("DescriptionNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("DescriptionNotEmpty").GetAwaiter().GetResult());
    }
}