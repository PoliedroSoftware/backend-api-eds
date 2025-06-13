using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Expenditures.Commands.UpdateExpenditures;
    public class UpdateExpendituresCommandValidator : AbstractValidator<UpdateExpendituresCommand>
    {
        //public UpdateExpendituresCommandValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Description) 
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("DescriptionNotNull").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("DescriptionNotEmpty").GetAwaiter().GetResult());
        //}
    }