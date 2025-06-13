using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Islander.Commands.CreateIslander;

public class CreateIslanderCommandValidator : AbstractValidator<CreateIslanderRequestDto>
{
    //public CreateIslanderCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Name)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NameNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NameNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.IdEds)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdEdsNotNull").GetAwaiter().GetResult())
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdEdsNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.Password)
    //       .NotNull().WithMessage(translationService.GetTranslationByKey("PasswordNotNull").GetAwaiter().GetResult())
    //       .NotEmpty().WithMessage(translationService.GetTranslationByKey("PasswordNotEmpty").GetAwaiter().GetResult());
    //}
}
