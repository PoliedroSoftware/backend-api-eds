using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.ShoppingProduct.Commands.UpdateShoppingProduct;
namespace Poliedro.Eds.Application.Shopping.Commands.UpdateShoppingProduct;
    public class UpdateShoppingProductCommandValidator : AbstractValidator<UpdateShoppingProductCommand>
    {
        //public UpdateShoppingProductCommandValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.IdShoppingProduct)
        //    .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("GreaterThanIdShoppingProduct").GetAwaiter().GetResult())
        //    .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdShoppingProductNotEmpty").GetAwaiter().GetResult());

        //    RuleFor(x => x.IdShopping)
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("GreaterThanIdShopping").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NotEmptyIdShopping").GetAwaiter().GetResult());

        //    RuleFor(x => x.IdProduct)
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdProductGreaterThan").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdProductNotEmpty").GetAwaiter().GetResult());

        //    RuleFor(x => x.Quantity)
        //        .GreaterThanOrEqualTo(0).WithMessage(translationService.GetTranslationByKey("QuantityGreaterThanOrEqualTo").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("QuantityNotEmpty").GetAwaiter().GetResult());

        //    RuleFor(x => x.Price)
        //        .GreaterThanOrEqualTo(0).WithMessage(translationService.GetTranslationByKey("PriceGreaterThanOrEqualTo").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("PriceNotEmpty").GetAwaiter().GetResult());

        //    RuleFor(x => x.IdCompartment)
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdCompartmentGreaterThan").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdCompartmentNotEmpty").GetAwaiter().GetResult());
        //}
    }

