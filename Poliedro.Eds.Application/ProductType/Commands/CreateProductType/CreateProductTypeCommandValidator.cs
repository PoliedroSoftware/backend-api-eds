using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.ProductType.Commands.CreateProductType;

public class CreateProductTypeCommandValidator : AbstractValidator<CreateProductTypeRequestDto>
{
    public CreateProductTypeCommandValidator(IRedisService redisService)
    {

        RuleFor(x => x.Description)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("DescriptionNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("DescriptionNotEmpty").GetAwaiter().GetResult());

    }


}
