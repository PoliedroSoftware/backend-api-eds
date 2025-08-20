using FluentValidation;
using Microsoft.Extensions.Hosting;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.HoseHistory.Commands.CreateHoseHistory;

public class CreateHoseHistoryCommandValidator : AbstractValidator<CreateHoseHistoryRequestDto>
{
    public CreateHoseHistoryCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.IdHose)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdHoseNotNull").GetAwaiter().GetResult())
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdHoseGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.AccumulatedGallons)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("AccumulatedGallonsNotNull").GetAwaiter().GetResult())
            .GreaterThan(0.0).WithMessage(redisService.GetValueFromCacheAsync("AccumulatedGallonsGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.AccumulatedAmount)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("AccumulatedAmountNotNull").GetAwaiter().GetResult())
            .GreaterThan(0.0).WithMessage(redisService.GetValueFromCacheAsync("AccumulatedAmountGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.IdDispensers)
           .NotNull().WithMessage(redisService.GetValueFromCacheAsync("IdDispensersNotNull").GetAwaiter().GetResult())
           .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdDispensersGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.Date)
           .NotNull().WithMessage(redisService.GetValueFromCacheAsync("DateNotNull").GetAwaiter().GetResult())
           .GreaterThan(DateTime.MinValue).WithMessage(redisService.GetValueFromCacheAsync("DateGreaterThan").GetAwaiter().GetResult());

    }
}
