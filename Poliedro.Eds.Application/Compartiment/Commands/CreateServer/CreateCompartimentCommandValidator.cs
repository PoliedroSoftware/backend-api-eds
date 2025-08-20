using FluentValidation;
using Poliedro.Eds.Application.Compartiment.Commands.CreateCompartiment;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;


namespace Poliedro.Eds.Application.Compartiment.CreateCompartiment;

public class CreateCompartimentCommandValidator : AbstractValidator<CreateCompartimentRequestDto>
{
    public CreateCompartimentCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.Number)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("NumberGreaterThan").GetAwaiter().GetResult());

        RuleFor(x => x.Nominal)
            .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("NominalGreaterThanOrEqualTo").GetAwaiter().GetResult());

        RuleFor(x => x.Operative)
            .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("OperativeGreaterThanOrEqualTo").GetAwaiter().GetResult())
            .LessThanOrEqualTo(x => x.Stock).WithMessage(redisService.GetValueFromCacheAsync("OperativeLessThanOrEqualTo").GetAwaiter().GetResult());

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("StockGreaterThanOrEqualTo").GetAwaiter().GetResult());

        RuleFor(x => x.Height)
            .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("HeightGreaterThanOrEqualTo").GetAwaiter().GetResult());

        RuleFor(x => x.IdTank)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdTankGreaterThan").GetAwaiter().GetResult());
    }

}
