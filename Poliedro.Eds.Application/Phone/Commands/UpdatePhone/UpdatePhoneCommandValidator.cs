using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.Phone.Commands.UpdatePhone;

public class UpdatePhoneCommandValidator : AbstractValidator<UpdatePhoneCommand>
{
    public UpdatePhoneCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.IdPhone)
            .NotNull().WithMessage("El ID del teléfono es requerido")
            .GreaterThan(0).WithMessage("El ID del teléfono debe ser mayor que 0");

        RuleFor(x => x.Number)
            .NotNull().WithMessage("El número de teléfono es requerido")
            .NotEmpty().WithMessage("El número de teléfono no puede estar vacío")
            .MaximumLength(13).WithMessage("El número de teléfono no puede exceder 13 caracteres");

        RuleFor(x => x.Name)
            .NotNull().WithMessage("El nombre es requerido")
            .NotEmpty().WithMessage("El nombre no puede estar vacío")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");
    }
}
