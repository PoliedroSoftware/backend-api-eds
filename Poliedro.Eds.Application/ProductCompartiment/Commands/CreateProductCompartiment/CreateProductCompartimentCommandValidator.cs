using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.ProductCompartiment.Commands.CreateProductCompartiment;

public class CreateProductCompartimentCommandValidator : AbstractValidator<CreateProductCompartimentRequestDto>
{
    //public CreateProductCompartimentCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.IdProduct)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdProductNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdProductNotEmpty").GetAwaiter().GetResult());
        
    //    RuleFor(x => x.IdCompartiment)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdCompartimentNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdCompartimentNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.Stock)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("StockNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("StockNotEmpty").GetAwaiter().GetResult());
    //}
}
