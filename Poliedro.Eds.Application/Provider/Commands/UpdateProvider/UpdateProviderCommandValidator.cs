using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Provider.Commands.UpdateProvider;

public class UpdateProviderCommandValidator : AbstractValidator<UpdateProviderCommand>
{
    //public UpdateProviderCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.IdProvider).NotNull().GreaterThan(0) 
    //         .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdProviderGreaterThan").GetAwaiter().GetResult());

    //    RuleFor(x => x.Name)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NameNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NameNotEmpty").GetAwaiter().GetResult());
    //}
}