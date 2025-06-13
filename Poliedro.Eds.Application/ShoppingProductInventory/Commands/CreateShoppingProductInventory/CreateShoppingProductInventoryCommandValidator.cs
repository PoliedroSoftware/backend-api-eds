using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Commands.CreateShoppingProductInventory
{
    public class CreateShoppingProductInventoryCommandValidator : AbstractValidator<CreateShoppingProductInventoryRequestDto>
    {
    //    public CreateShoppingProductInventoryCommandValidator(ITranslationService translationService)
    //    {
    //        RuleFor(x => x.IdShoppingProductInventory)
    //       .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdShoppingProductInventoryGreaterThan").GetAwaiter().GetResult())
    //       .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdShoppingProductInventoryNotEmpty").GetAwaiter().GetResult());

    //        RuleFor(x => x.IdShoppingProduct)
    //            .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdShoppingProductGreaterThan").GetAwaiter().GetResult())
    //            .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdShoppingProductNotEmpty").GetAwaiter().GetResult());

    //        RuleFor(x => x.IdInventory)
    //            .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdInventoryGreaterThan").GetAwaiter().GetResult())
    //            .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdInventoryNotEmpty").GetAwaiter().GetResult());
    //    }
    }
}
