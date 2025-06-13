using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.ProductType.Commands.CreateProductType;

public class CreateProductTypeCommandValidator : AbstractValidator<CreateProductTypeRequestDto>
{
    //public CreateProductTypeCommandValidator(ITranslationService translationService)
    //{

    //    RuleFor(x => x.Description)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("DescriptionNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("DescriptionNotEmpty").GetAwaiter().GetResult());

    //}


}
