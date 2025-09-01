using FluentValidation;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Poliedro.Eds.Application.Compartiment.Commands.UpdateCompartiment
{
    public class UpdateCompartimentCommandValidator : AbstractValidator<UpdateCompartimentCommand>
    {
        public UpdateCompartimentCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.IdCompartment)
           .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdCompartmentGreaterThan").GetAwaiter().GetResult())
           .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdCompartmentNotEmpty").GetAwaiter().GetResult());

            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("NumberGreaterThan").GetAwaiter().GetResult());

            RuleFor(x => x.Nominal)
                .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("NominalGreaterThanOrEqualTo").GetAwaiter().GetResult());
            RuleFor(x => x.Operative)
<<<<<<< HEAD
                .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("OperativeGreaterThanOrEqualTo").GetAwaiter().GetResult());

            RuleFor(x => x.IdProduct)
                .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("IdProductGreaterThanOrEqualTo").GetAwaiter().GetResult());
=======
                .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("OperativeGreaterThanOrEqualTo").GetAwaiter().GetResult())
                .LessThanOrEqualTo(x => x.Stock).WithMessage(redisService.GetValueFromCacheAsync("OperativeGreaterThanOrEqualTo").GetAwaiter().GetResult());

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("StockGreaterThanOrEqualTo").GetAwaiter().GetResult());
>>>>>>> New-service-StrongBox

            RuleFor(x => x.Height)
                .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("HeightGreaterThanOrEqualTo").GetAwaiter().GetResult());

            RuleFor(x => x.IdTank)
                .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdTankGreaterThan").GetAwaiter().GetResult())
                .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdTankNotEmpty").GetAwaiter().GetResult());
        }
    }
}
