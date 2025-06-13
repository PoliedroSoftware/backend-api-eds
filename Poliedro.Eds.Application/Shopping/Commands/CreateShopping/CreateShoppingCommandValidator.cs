using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
using Poliedro.Eds.Application.Shopping.Commands.UpdateShopping;

namespace Poliedro.Eds.Application.Shopping.Shopping.CreateShopping;

public class CreateShoppingCommandValidator : AbstractValidator<CreateShoppingRequestDto>
{
    //public CreateShoppingCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Invoice)
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("InvoiceNotEmpty)").GetAwaiter().GetResult())
    //        .MaximumLength(45).WithMessage(translationService.GetTranslationByKey("InvoiceMaximumLength)").GetAwaiter().GetResult());

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
