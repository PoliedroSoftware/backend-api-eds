using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.TypeOfCollection.Commands.CreateTypeOfCollection;

public class CreateTypeOfCollectionCommandValidator : AbstractValidator<CreateTypeOfCollectionRequestDto>
{
    //public CreateTypeOfCollectionCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Description)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("DescriptionNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("DescriptionNotEmpty").GetAwaiter().GetResult());
    //}


}