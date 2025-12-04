using FluentValidation;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Poliedro.Eds.Application.Compartiment.Commands.UpdateCompartiment
{
    public class UpdateCompartimentCommandValidator : AbstractValidator<UpdateCompartimentCommand>
    {
        private static string ResolveMessage(IRedisService redisService, string key, string fallback)
        {
            try
            {
                var value = redisService?.GetValueFromCacheAsync(key).GetAwaiter().GetResult();
                return string.IsNullOrWhiteSpace(value) ? fallback : value!;
            }
            catch
            {
                return fallback;
            }
        }

        public UpdateCompartimentCommandValidator(IRedisService redisService)
        {
            RuleFor(x => x.IdCompartment)
               .GreaterThan(0).WithMessage(ResolveMessage(redisService, "IdCompartmentGreaterThan", "El Id del compartimento debe ser mayor que 0"))
               .NotEmpty().WithMessage(ResolveMessage(redisService, "IdCompartmentNotEmpty", "El Id del compartimento no puede estar vacío"));

            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage(ResolveMessage(redisService, "NumberGreaterThan", "El número debe ser mayor que 0"));

            RuleFor(x => x.Nominal)
                .GreaterThanOrEqualTo(0).WithMessage(ResolveMessage(redisService, "NominalGreaterThanOrEqualTo", "El nominal debe ser mayor o igual a 0"));
            RuleFor(x => x.Operative)
                .GreaterThanOrEqualTo(0).WithMessage(ResolveMessage(redisService, "OperativeGreaterThanOrEqualTo", "El operativo debe ser mayor o igual a 0"));

            RuleFor(x => x.IdProduct)
                .GreaterThanOrEqualTo(0).WithMessage(ResolveMessage(redisService, "IdProductGreaterThanOrEqualTo", "El Id del producto debe ser mayor o igual a 0"));

            RuleFor(x => x.Height)
                .GreaterThanOrEqualTo(0).WithMessage(ResolveMessage(redisService, "HeightGreaterThanOrEqualTo", "La altura debe ser mayor o igual a 0"));

            RuleFor(x => x.IdTank)
                .GreaterThan(0).WithMessage(ResolveMessage(redisService, "IdTankGreaterThan", "El Id del tanque debe ser mayor que 0"))
                .NotEmpty().WithMessage(ResolveMessage(redisService, "IdTankNotEmpty", "El Id del tanque no puede estar vacío"));
        }
    }
}
