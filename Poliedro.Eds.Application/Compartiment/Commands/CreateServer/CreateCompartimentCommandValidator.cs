using FluentValidation;
using Poliedro.Eds.Application.Compartiment.Commands.CreateCompartiment;
using Poliedro.Eds.Application.Ports.Translations;


namespace Poliedro.Eds.Application.Compartiment.CreateCompartiment;

public class CreateCompartimentCommandValidator : AbstractValidator<CreateCompartimentRequestDto>
{
    //public CreateCompartimentCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Number)
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("NumberGreaterThan").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.Nominal)
    //        .GreaterThanOrEqualTo(0).WithMessage(translationService.GetTranslationByKey("NominalGreaterThanOrEqualTo").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.Operative)
    //        .GreaterThanOrEqualTo(0).WithMessage(translationService.GetTranslationByKey("OperativeGreaterThanOrEqualTo").GetAwaiter().GetResult()) 
    //        .LessThanOrEqualTo(x => x.Stock).WithMessage(translationService.GetTranslationByKey("OperativeLessThanOrEqualTo").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.Stock)
    //        .GreaterThanOrEqualTo(0).WithMessage(translationService.GetTranslationByKey("StockGreaterThanOrEqualTo").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.Height)
    //        .GreaterThanOrEqualTo(0).WithMessage(translationService.GetTranslationByKey("HeightGreaterThanOrEqualTo").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.IdTank)
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdTankGreaterThan").GetAwaiter().GetResult()); 
    //}

}
