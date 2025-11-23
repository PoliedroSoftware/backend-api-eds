using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Poliedro.Eds.Application.Hose.Commands.CreateHose;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.PosOfSale.Commands.CreatePosOfSale;

public class CreatePosOfSaleCommandValidator : AbstractValidator<CreatePosOfSaleRequestDto>
{
    public CreatePosOfSaleCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.InvoiceNumber)
            .NotNull().WithMessage(redisService.GetValueFromCacheAsync("NumberNotNull").GetAwaiter().GetResult());
    }
}
