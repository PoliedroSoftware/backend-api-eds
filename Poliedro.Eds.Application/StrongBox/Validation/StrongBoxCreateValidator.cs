using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.StrongBox.Repositories;
using Poliedro.Eds.Domain.StrongBox.ValueObjects;

namespace Poliedro.Eds.Application.StrongBox.Validation;
public class StrongBoxCreateValidator : AbstractValidator<StrongBoxDtoCreateRequest>
{
    public StrongBoxCreateValidator(IStrongBoxRepositoryGetLast repoGetLast)
    {
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("El campo Tipo es obligatorio.")
            .Must(v => StrongBoxType.IsValid(v.Trim().ToUpperInvariant()))
            .WithMessage("El campo Tipo debe ser 'CORTE' o 'RETIRO'.");

        RuleFor(x => x.Note)
            .MaximumLength(500)
            .WithMessage("El campo Nota no debe exceder los 500 caracteres.");

        RuleFor(x => x.Ammount)
            .NotEmpty().WithMessage("El campo Monto es obligatorio.")
            .GreaterThan(0).WithMessage("El campo Monto debe ser mayor que 0.");

        RuleFor(x => x)
            .MustAsync(async (req, CancellationToken) =>
            {
                if (req?.Type?.Trim().ToUpperInvariant() == StrongBoxType.RETIRO)
                {
                    var last = await repoGetLast.GetLastAsync(CancellationToken);
                    var balance = last?.Saldo ?? 0.0;
                    return req.Ammount <= balance;
                }
                return true;

            }).WithMessage("No se puede hacer el retiro el saldo no puede estar en negativo!!!");
    }
}
