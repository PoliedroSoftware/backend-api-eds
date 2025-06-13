using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.TypeOfCollection.Commands.UpdateTypeOfCollection;
    public class UpdateTypeOfCollectionCommandValidator : AbstractValidator<UpdateTypeOfCollectionCommand>
    {
        //public UpdateTypeOfCollectionCommandValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Description)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("DescriptionNotNull").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("DescriptionNotEmpty").GetAwaiter().GetResult());
        //}
    }