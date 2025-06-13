using FluentValidation;
using Poliedro.Eds.Application.Eds.Queries.GetEdsById;
using Poliedro.Eds.Application.Ports.Translations;
using System.Net;

namespace Poliedro.Eds.Application.Eds.Commands.CreateEds;

public class CreateEdsCommandValidator : AbstractValidator<CreateEdsRequestDto>
{
    //public CreateEdsCommandValidator(ITranslationService translationService)
    //{

    //    RuleFor(x => x.Name)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NameNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NameNotEmpty").GetAwaiter().GetResult())
    //        .NotEqual("string").WithMessage(translationService.GetTranslationByKey("NameNotEqual").GetAwaiter().GetResult());

    //    RuleFor(x => x.Nit)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NitNotNull").GetAwaiter().GetResult()) 
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NitNotEmpty").GetAwaiter().GetResult()) 
    //        .NotEqual("string").WithMessage(translationService.GetTranslationByKey("NitNotEqual").GetAwaiter().GetResult());

    //    RuleFor(x => x.Address)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("AddressNotNull").GetAwaiter().GetResult()) 
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("AddressNotEmpty").GetAwaiter().GetResult())
    //        .NotEqual("string").WithMessage(translationService.GetTranslationByKey("AddressNotEqual").GetAwaiter().GetResult());

    //    RuleFor(x => x.Sicom)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("SicomNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("SicomNotEmpty").GetAwaiter().GetResult())
    //        .NotEqual("string").WithMessage(translationService.GetTranslationByKey("SicomNotEqual").GetAwaiter().GetResult());

    //    RuleFor(x => x.IdBusiness)
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdBusinessGreaterThan").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdBusinessNotEmpty").GetAwaiter().GetResult());

    //}
    //public class GetEdsByIdCommandValidator : AbstractValidator<GetEdsByIdQuery>
    //{
    //    public GetEdsByIdCommandValidator(ITranslationService translationService)
    //    {
    //        RuleFor(x => x.Id)
    //            .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdGreaterThan").GetAwaiter().GetResult());
    //    }
    //}
}
