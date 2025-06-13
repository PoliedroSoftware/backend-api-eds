using FluentValidation;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Eds.Commands.UpdateEds;

public class UpdateEdsCommandValidator : AbstractValidator<UpdateEdsCommand>
{
    //public UpdateEdsCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.IdEds).NotNull().GreaterThan(0) 
    //         .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdEdsGreaterThan").GetAwaiter().GetResult());

    //    RuleFor(x => x.Name)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NameNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NameNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.Nit)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("NitNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("NitNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.Address)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("AddressNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("AddressNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.Sicom)
    //        .NotNull().WithMessage(translationService.GetTranslationByKey("SicomNotNull").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("SicomNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.IdBusiness)
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdBusinessGreaterThan").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdBusinessNotEmp").GetAwaiter().GetResult());

    //}
}