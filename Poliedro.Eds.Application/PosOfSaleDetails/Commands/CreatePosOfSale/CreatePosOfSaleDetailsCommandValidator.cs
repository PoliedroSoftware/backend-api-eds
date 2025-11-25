using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Poliedro.Eds.Application.Hose.Commands.CreateHose;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Commands.CreatePosOfSale;

public class CreatePosOfSaleDetailsCommandValidator : AbstractValidator<CreatePosOfSaleDetailsRequestDto>
{
    public CreatePosOfSaleDetailsCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.PosId)
       .NotNull().WithMessage(redisService.GetValueFromCacheAsync("PosIdNotNull").GetAwaiter().GetResult())
       .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("PosIdNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.ProductCode)
        .NotNull().WithMessage(redisService.GetValueFromCacheAsync("ProductCodeNotNull").GetAwaiter().GetResult());

        RuleFor(x => x.ProductName)
        .NotNull().WithMessage(redisService.GetValueFromCacheAsync("ProductNameNotNull").GetAwaiter().GetResult());
    }
}
