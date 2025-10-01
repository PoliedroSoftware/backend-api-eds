using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Phone.DomainServices.GetByNumber;

namespace Poliedro.Eds.Application.Phone.Commands.CreatePhone;

public class CreatePhoneCommandValidator : AbstractValidator<CreatePhoneRequestDto>
{
    public CreatePhoneCommandValidator(IRedisService redisService, IPhoneGetByNumberService phoneGetByNumberService)
    {
        RuleForEach(x => x.Phones).ChildRules(phone =>
        {
            phone.RuleFor(p => p.Number)
                .NotNull().WithMessage("El número de teléfono es requerido")
                .NotEmpty().WithMessage("El número de teléfono no puede estar vacío")
                .MaximumLength(13).WithMessage("El número de teléfono no puede exceder 13 caracteres")
                .MustAsync(async (number, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(number))
                        return true; // Let other validators handle null/empty validation

                    // Normalize the number first (same logic as in the handler)
                    var normalizedNumber = NormalizePhoneNumber(number);
                    return !await phoneGetByNumberService.ExistsAsync(normalizedNumber);
                })
                .WithMessage("El número de teléfono {PropertyValue} ya existe en la base de datos");

            phone.RuleFor(p => p.Name)
                .NotNull().WithMessage("El nombre es requerido")
                .NotEmpty().WithMessage("El nombre no puede estar vacío")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");
        });

        // Validate for duplicate numbers within the same request
        RuleFor(x => x.Phones)
            .Must(phones =>
            {
                if (phones == null || !phones.Any()) return true;

                var normalizedNumbers = phones.Select(p => NormalizePhoneNumber(p.Number)).ToList();
                return normalizedNumbers.Count == normalizedNumbers.Distinct().Count();
            })
            .WithMessage("No se pueden enviar números de teléfono duplicados en la misma solicitud");
    }

    private static string NormalizePhoneNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number)) return number;

        var digits = new string(number.Where(char.IsDigit).ToArray());

        if (digits.StartsWith("57") && digits.Length == 12)
            return digits;

        if (digits.StartsWith("0"))
            digits = digits.Substring(1);

        if (!digits.StartsWith("57"))
            digits = "57" + digits;

        return digits;
    }
}
