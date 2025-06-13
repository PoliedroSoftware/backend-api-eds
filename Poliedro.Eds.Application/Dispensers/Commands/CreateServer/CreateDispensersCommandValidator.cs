using FluentValidation;
using Poliedro.Eds.Application.Dispensers.Commands.CreateDispensers;
using Poliedro.Eds.Application.Ports.Translations;


namespace Poliedro.Eds.Application.Dispensers.Dispensers.CreateDispensers;

public class CreateDispensersCommandValidator : AbstractValidator<CreateDispensersRequestDto>
{
    //public CreateDispensersCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.Code)
    //.NotEmpty().WithMessage(translationService.GetTranslationByKey("CodeNotEmpty").GetAwaiter().GetResult()) 
    //.MaximumLength(50).WithMessage(translationService.GetTranslationByKey("CodeMaximumLength").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.Number)
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("NumberGreaterThan").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.DispenserTypeId)
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("DispenserTypeIdGreaterThan").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.HoseNumber)
    //        .GreaterThanOrEqualTo(1).WithMessage(translationService.GetTranslationByKey("HoseNumberGreaterThanOrEqualTo").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.EdsId)
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("EdsIdGreaterThan").GetAwaiter().GetResult()); 

    //    RuleFor(x => x.IdIsland)
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdIslandGreaterThan").GetAwaiter().GetResult()) 
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdIslandNotEmpty").GetAwaiter().GetResult());  
    //}

}
