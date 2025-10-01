using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Phone.DomainServices.GetByNumber;

namespace Poliedro.Eds.Application.Phone.Commands.UpdatePhone;

public class UpdatePhoneCommandValidator : AbstractValidator<UpdatePhoneCommand>
{
    public UpdatePhoneCommandValidator(IRedisService redisService, IPhoneGetByNumberService phoneGetByNumberService)
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

        // Add unique number validation for updates
        RuleFor(x => x)
            .MustAsync(async (command, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(command.Number))
                    return true; // Let other validators handle null/empty validation

                // Normalize the number first
                var normalizedNumber = NormalizePhoneNumber(command.Number);
                var existingPhone = await phoneGetByNumberService.GetByNumberAsync(normalizedNumber);
                
                if (existingPhone.IsSuccess && existingPhone.Value != null)
                {
                    // Allow if the number belongs to the same phone being updated
                    return existingPhone.Value.IdPhone == command.IdPhone;
                }
                
                // Number doesn't exist, so it's unique
                return true;
            })
            .WithMessage("El número de teléfono ya existe en la base de datos")
            .WithName("Number");
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
