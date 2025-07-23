using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.Provider.Queries.GetProviderById;

namespace Poliedro.Eds.Application.Provider.Commands.CreateProvider;

public class CreateProviderCommandValidator : AbstractValidator<CreateProviderRequestDto>
{
    public CreateProviderCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NameNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NameNotEmpty").GetAwaiter().GetResult())
            .NotEqual("string").WithMessage(redisService.GetValueFromCacheAsync("NameNotEqual").GetAwaiter().GetResult());
    }
    public class GetProviderByIdCommandValidator : AbstractValidator<GetProviderByIdQuery>
    {
        public GetProviderByIdCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdGreaterThan").GetAwaiter().GetResult());
        }
    }
}
