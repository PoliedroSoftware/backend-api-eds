using FluentValidation;
using Poliedro.Eds.Application.Category.Commands.CreateCategory;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Category.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryRequestDto>
{
    public CreateCategoryCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.Description)
           .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("DescriptionNotEmpty").GetAwaiter().GetResult())
           .MaximumLength(45).WithMessage(redisService.GetValueFromCacheAsync("DescriptionMaximumLength").GetAwaiter().GetResult())
           .Matches(@"^[a-zA-Z0-9\s]+$").WithMessage(redisService.GetValueFromCacheAsync("DescriptionMatches").GetAwaiter().GetResult());
    }
}
