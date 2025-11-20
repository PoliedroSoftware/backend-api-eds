using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Poliedro.Eds.Application.RegisterShift.Dtos;

namespace Poliedro.Eds.Application.RegisterShift.Validations;

public class RegisterShiftCreateValidator : AbstractValidator<RegisterShiftDto>
{
    public RegisterShiftCreateValidator()
    {
        RuleFor(x => x.IdEds)
            .NotEmpty().WithMessage("El campo IdEds es obligatorio.")
            .NotNull().WithMessage("El campo IdEds no puede ser nulo.");

        RuleFor(x => x.IdBusiness)
            .NotEmpty().WithMessage("El campo IdBusiness es obligatorio")
            .NotNull().WithMessage("El campo IdBsiness no puede ser null.");

        RuleFor(x => x.IdIslander)
            .NotEmpty().WithMessage("El campo IdIslander es obligatorio.")
            .NotNull().WithMessage("El campo IdIslander no puede estar null.");

        RuleFor(x => x.DateStartTime)
            .NotEmpty().WithMessage("El campo DateStartTime es obligatorio.")
            .NotNull().WithMessage("El campo DateStartTime no puede ser nulo.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("El campo StartTime es obligatorio.")
            .NotNull().WithMessage("El campo StartTime no puede ser nulo.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("El campo EndTime es obligatorio.")
            .NotNull().WithMessage("El campo EndTime no puede ser nulo.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("El campo StartTime es obligatorio.")
            .NotNull().WithMessage("El campo StartTime no puede ser nulo.");
    }
}
