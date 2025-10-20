using FluentValidation;
using Poliedro.Eds.Domain.TransferValidation.ValueObjects;

namespace Poliedro.Eds.Application.TransferValidation.Commands.UpdateTransferValidation;

public class UpdateTransferValidationValidator : AbstractValidator<UpdateTransferValidationRequestDto>
{
    public UpdateTransferValidationValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .WithMessage("El nombre del cliente es requerido")
            .MaximumLength(150)
            .WithMessage("El nombre del cliente no puede exceder 150 caracteres");

        RuleFor(x => x.TransactionAmount)
            .GreaterThan(0)
            .WithMessage("El monto de la transacción debe ser mayor a cero")
            .LessThanOrEqualTo(999999999.99)
            .WithMessage("El monto de la transacción excede el límite permitido");

        RuleFor(x => x.TransactionDate)
            .NotEmpty()
            .WithMessage("La fecha de la transacción es requerida")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("La fecha de la transacción no puede ser futura");

        RuleFor(x => x.TransactionTime)
            .NotEmpty()
            .WithMessage("La hora de la transacción es requerida");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("El estado es requerido")
            .Must(status => TransferValidationStatus.IsValid(status))
            .WithMessage($"Estado inválido. Valores válidos: {string.Join(", ", TransferValidationStatus.GetValidStatuses())}");

        RuleFor(x => x.ConfirmedBy)
            .MaximumLength(255)
            .WithMessage("El campo 'Confirmado por' no puede exceder 255 caracteres")
            .NotEmpty()
            .When(x => !string.IsNullOrWhiteSpace(x.Status) && 
                      x.Status.Trim().ToUpperInvariant() == TransferValidationStatus.CONFIRMADA)
            .WithMessage("Debe especificar quién confirmó la transacción cuando el estado es CONFIRMADA");
    }
}
