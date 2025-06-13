using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.ShoppingProduct.Queries.GetShoppingProductById;
using Poliedro.Eds.Application.ShoppingProductInventory.Queries.GetShoppingProductInventoryById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Queries.GetShoppingProductInventoryById
{
    public class GetShoppingProductInventoryByIdQueryValidator : AbstractValidator<GetShoppingProductInventoryByIdQuery>
    {
        //public GetShoppingProductInventoryByIdQueryValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.Id)
        //        .NotNull().WithMessage(translationService.GetTranslationByKey("IdNotNull").GetAwaiter().GetResult())
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
        //}
    }
}
