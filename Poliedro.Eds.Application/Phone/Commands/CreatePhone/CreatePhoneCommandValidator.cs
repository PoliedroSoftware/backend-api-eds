using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.Phone.Commands.CreatePhone;

public class CreatePhoneCommandValidator : AbstractValidator<CreatePhoneRequestDto>
{
    public CreatePhoneCommandValidator(IRedisService redisService)
    {
        RuleForEach(x => x.Phones).ChildRules(phone =>
        {
            phone.RuleFor(p => p.Number)
                .NotNull().WithMessage("El número de teléfono es requerido")
                .NotEmpty().WithMessage("El número de teléfono no puede estar vacío")
                .MaximumLength(13).WithMessage("El número de teléfono no puede exceder 13 caracteres");

            phone.RuleFor(p => p.Name)
                .NotNull().WithMessage("El nombre es requerido")
                .NotEmpty().WithMessage("El nombre no puede estar vacío")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");
        });
    }
}
