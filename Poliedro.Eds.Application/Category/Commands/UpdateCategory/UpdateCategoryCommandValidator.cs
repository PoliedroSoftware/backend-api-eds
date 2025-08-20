using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Category.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.IdCategory)
        .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdCategoryGreaterThan").GetAwaiter().GetResult())
        .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdCategoryNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("DescriptionNotEmpty").GetAwaiter().GetResult())
            .MaximumLength(45).WithMessage(redisService.GetValueFromCacheAsync("DescriptionMaximumLength").GetAwaiter().GetResult())
            .Matches(@"^[a-zA-Z0-9\s]+$").WithMessage(redisService.GetValueFromCacheAsync("DescriptionMatches").GetAwaiter().GetResult());
    }
}

