using FluentValidation;
using Poliedro.Eds.Application.Bank.Dtos;
using Poliedro.Eds.Domain.Bank.ValueObjects;

namespace Poliedro.Eds.Application.Bank.Validation;

public class BankCreateValidator : AbstractValidator<BankDtoCreateRequest>
{
    public BankCreateValidator()
    {
        RuleFor(x => x.IdAccount)
            .GreaterThan(0)
            .WithMessage("IdAccount debe ser mayor que 0");

        RuleFor(x => x.IdEds)
            .GreaterThan(0)
            .WithMessage("IdEds debe ser mayor que 0");

        RuleFor(x => x.Moviment)
            .NotEmpty()
            .WithMessage("El tipo de movimiento es requerido")
            .Must(type => BankMovementType.IsValid(type))
            .WithMessage($"Tipo de movimiento inválido. Valores válidos: {string.Join(", ", BankMovementType.GetValidTypes())}");

        RuleFor(x => x.Ammount)
            .GreaterThan(0)
            .WithMessage("El monto debe ser mayor que 0");

        RuleFor(x => x.Note)
            .NotEmpty()
            .WithMessage("La nota es requerida")
            .MaximumLength(500)
            .WithMessage("La nota no puede exceder 500 caracteres");
    }
}
