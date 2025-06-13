using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Product.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductRequestDto>
{
    //public CreateProductCommandValidator(ITranslationService translationService)
    //{

    //    RuleFor(x => x.Name)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NameNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NameNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.IdProductType)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdProductTypeNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdProductTypeNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.Price)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("PriceNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("PriceNotEmpty").GetAwaiter().GetResult());
    //}


}
