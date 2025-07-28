using System.Net;
using FluentValidation;
using Poliedro.Eds.Application.Eds.Queries.GetEdsById;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Eds.Commands.CreateEds;

public class CreateEdsCommandValidator : AbstractValidator<CreateEdsRequestDto>
{
    public CreateEdsCommandValidator(IRedisService redisService)
    {

        RuleFor(x => x.Name)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NameNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NameNotEmpty").GetAwaiter().GetResult())
            .NotEqual("string").WithMessage(redisService.GetValueFromCacheAsync("NameNotEqual").GetAwaiter().GetResult());

        RuleFor(x => x.Nit)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NitNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NitNotEmpty").GetAwaiter().GetResult())
            .NotEqual("string").WithMessage(redisService.GetValueFromCacheAsync("NitNotEqual").GetAwaiter().GetResult());

        RuleFor(x => x.Address)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("AddressNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("AddressNotEmpty").GetAwaiter().GetResult())
            .NotEqual("string").WithMessage(redisService.GetValueFromCacheAsync("AddressNotEqual").GetAwaiter().GetResult());

        RuleFor(x => x.Sicom)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("SicomNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("SicomNotEmpty").GetAwaiter().GetResult())
            .NotEqual("string").WithMessage(redisService.GetValueFromCacheAsync("SicomNotEqual").GetAwaiter().GetResult());

        RuleFor(x => x.IdBusiness)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdBusinessGreaterThan").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdBusinessNotEmpty").GetAwaiter().GetResult());

    }
    public class GetEdsByIdCommandValidator : AbstractValidator<GetEdsByIdQuery>
    {
        public GetEdsByIdCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdGreaterThan").GetAwaiter().GetResult());
        }
    }
}
