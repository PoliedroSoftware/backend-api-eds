using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.Provider.Queries.GetProviderById;

namespace Poliedro.Eds.Application.Provider.Commands.CreateProvider;

public class CreateProviderCommandValidator : AbstractValidator<CreateProviderRequestDto>
{
    //public CreateProviderCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Name)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NameNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NameNotNull").GetAwaiter().GetResult())
    //        .NotEqual("string").WithMessage(translationService.GetTranslationByKey("NameNotNull").GetAwaiter().GetResult());
    //}
    //public class GetProviderByIdCommandValidator : AbstractValidator<GetProviderByIdQuery>
    //{
    //    public GetProviderByIdCommandValidator(ITranslationService translationService)
    //    {
    //        RuleFor(x => x.Id)
    //            .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
    //    }
    //}
}
