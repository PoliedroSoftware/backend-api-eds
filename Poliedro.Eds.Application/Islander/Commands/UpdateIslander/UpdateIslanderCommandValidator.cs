using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Islander.Commands.UpdateIslander
{
    public class UpdateIslanderCommandValidator : AbstractValidator<UpdateIslanderCommand>
    {
        //public UpdateIslanderCommandValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Name)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("NameNotNull").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NameNotEmpty").GetAwaiter().GetResult());

        //    RuleFor(x => x.IdEds).NotNull().GreaterThan(0)
        //         .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdEdsGreaterThan").GetAwaiter().GetResult());
        //}
    }
}
