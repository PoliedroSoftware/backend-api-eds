using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;
namespace Poliedro.Eds.Application.Category.Commands.UpdateCategory;
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
    //    public UpdateCategoryCommandValidator(ITranslationService translationService)
    //    {
    //    RuleFor(x => x.IdCategory)
    //    .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdCategoryGreaterThan").GetAwaiter().GetResult())
    //    .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdCategoryNotEmpty").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.Description)
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("DescriptionNotEmpty").GetAwaiter().GetResult()) 
    //        .MaximumLength(45).WithMessage(translationService.GetTranslationByKey("DescriptionMaximumLength").GetAwaiter().GetResult()) 
    //        .Matches(@"^[a-zA-Z0-9\s]+$").WithMessage(translationService.GetTranslationByKey("DescriptionMatches").GetAwaiter().GetResult()); 
    //}
    }

