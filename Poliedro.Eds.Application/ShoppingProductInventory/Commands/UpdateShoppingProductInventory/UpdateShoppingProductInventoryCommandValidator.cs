using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Commands.UpdateShoppingProductInventory
{
    public class UpdateShoppingProductInventoryCommandValidator :  AbstractValidator<UpdateShoppingProductInventoryCommand>
    {
        //public UpdateShoppingProductInventoryCommandValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.IdShopping)
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdShoppingGreaterThan").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdShoppingNotEmpty").GetAwaiter().GetResult());

        //    RuleFor(x => x.IdInventory)
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdInventoryGreaterThan").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdInventoryNotEmpty").GetAwaiter().GetResult());

        //    RuleFor(x => x.IdShoppingProduct)
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdShoppingProductGreaterThan").GetAwaiter().GetResult())
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdShoppingProductNotEmpty").GetAwaiter().GetResult());
        //}
    }
}
