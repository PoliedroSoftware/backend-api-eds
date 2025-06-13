using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;
namespace Poliedro.Eds.Application.Shopping.Commands.UpdateShopping;
    public class UpdateShoppingCommandValidator : AbstractValidator<UpdateShoppingCommand>
    {
        //public UpdateShoppingCommandValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.IdShopping)
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdShoppingGreaterThan)").GetAwaiter().GetResult());

        //    RuleFor(x => x.Invoice)
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("InvoiceNotEmpty").GetAwaiter().GetResult())
        //        .MaximumLength(45).WithMessage(translationService.GetTranslationByKey("InvoiceMaximumLength").GetAwaiter().GetResult());

        //    RuleFor(x => x.Date)
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("DateNotEmpty").GetAwaiter().GetResult())
        //        .Must(date => date != default(DateTime)).WithMessage(translationService.GetTranslationByKey("DateMust").GetAwaiter().GetResult());

        //    RuleFor(x => x.IdProvider)
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdProviderGreaterThan").GetAwaiter().GetResult());

        //    RuleFor(x => x.IdCategory)
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdCategoryGreaterThan").GetAwaiter().GetResult());

        //    RuleFor(x => x.Amount)
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("AmountGreaterThan").GetAwaiter().GetResult());
        //}
    }

