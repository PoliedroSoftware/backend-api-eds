using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Expenditures.Commands.CreateExpenditures;

public class CreateExpendituresCommandValidator : AbstractValidator<CreateExpendituresRequestDto>
{
    //public CreateExpendituresCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Description)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("DescriptionNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("DescriptionNotEmpty").GetAwaiter().GetResult());
    //}


}
