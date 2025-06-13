using FluentValidation;
using Poliedro.Eds.Application.Category.Commands.CreateCategory;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Category.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryRequestDto>
{
    //public CreateCategoryCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Description)
    //       .NotEmpty().WithMessage(translationService.GetTranslationByKey("DescriptionNotEmpty").GetAwaiter().GetResult()) 
    //       .MaximumLength(45).WithMessage(translationService.GetTranslationByKey("DescriptionMaximumLength").GetAwaiter().GetResult()) 
    //       .Matches(@"^[a-zA-Z0-9\s]+$").WithMessage(translationService.GetTranslationByKey("DescriptionMatches").GetAwaiter().GetResult());
    //}
}
