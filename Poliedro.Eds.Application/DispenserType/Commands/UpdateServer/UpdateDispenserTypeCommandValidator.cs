using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;
namespace Poliedro.Eds.Application.DispenserType.Commands.UpdateDispenserType
{
    public class UpdateDispenserTypeCommandValidator : AbstractValidator<UpdateDispenserTypeCommand>
    {
    //    public UpdateDispenserTypeCommandValidator(ITranslationService translationService)
    //    {
    //        RuleFor(x => x.IdType) 
    //.NotNull().WithMessage(translationService.GetTranslationByKey("IdTypeNotNull").GetAwaiter().GetResult()) // Asegura que no sea null
    //.GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdTypeGreaterThan").GetAwaiter().GetResult()); // Debe ser mayor que 0

    //        RuleFor(x => x.Description) 
    //            .NotEmpty().WithMessage(translationService.GetTranslationByKey("DescriptionNotEmpty").GetAwaiter().GetResult()) 
    //            .MaximumLength(50).WithMessage(translationService.GetTranslationByKey("DescriptionMaximumLength").GetAwaiter().GetResult()); 
    //    }
    }
}
