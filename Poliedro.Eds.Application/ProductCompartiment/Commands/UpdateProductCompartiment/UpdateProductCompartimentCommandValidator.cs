using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.ProductCompartiment.Commands.UpdateProductCompartiment
{
    public class UpdateProductCompartimentCommandValidator : AbstractValidator<UpdateProductCompartimentCommand>
    {
        //public UpdateProductCompartimentCommandValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.IdProduct)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdProductNotNull").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdProductNotEmpty").GetAwaiter().GetResult());
            
        //    RuleFor(x => x.IdProductCompartiment)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdProductCompartimentNotNull").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdProductCompartimentNotEmpty").GetAwaiter().GetResult());
            
        //    RuleFor(x => x.Stock)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("StockNotNull").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("StockNotEmpty").GetAwaiter().GetResult());
        //}
    }
}
