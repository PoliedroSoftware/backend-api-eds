using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Eds.Commands.UpdateEds;

public class UpdateEdsCommandValidator : AbstractValidator<UpdateEdsCommand>
{
    public UpdateEdsCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.IdEds).NotNull().GreaterThan(0)
             .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdEdsGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.Name)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NameNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NameNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Nit)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NitNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("NitNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Address)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("AddressNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("AddressNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Sicom)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("SicomNotNull").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("SicomNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.IdBusiness)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdBusinessGreaterThan").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdBusinessNotEmp").GetAwaiter().GetResult());

    }
}