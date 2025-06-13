using FluentValidation;
using Poliedro.Eds.Application.Court.Commands.CreateCourt;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.CompartimentCapacity.Commands.UpdateCompartimentCapacity;
    public class UpdateCompartimentCapacityCommandValidator : AbstractValidator<UpdateCompartimentCapacityCommand>
    {
    //    public UpdateCompartimentCapacityCommandValidator(ITranslationService translationService)
    //{
    //        RuleFor(x => x.IdCompartiment)
    //            .NotNull().WithMessage(translationService.GetTranslationByKey("IdCompartimentNotNull").GetAwaiter().GetResult()) 
    //            .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdCompartimentNotEmpty").GetAwaiter().GetResult()); 

    //        RuleFor(x => x.IdCapacity)
    //            .NotNull().WithMessage(translationService.GetTranslationByKey("IdCapacityNotNull").GetAwaiter().GetResult()) 
    //            .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdCapacityNotEmpty").GetAwaiter().GetResult()); 

    //        RuleFor(x => x.Default)
    //            .NotNull().WithMessage(translationService.GetTranslationByKey("DefaultNotNull").GetAwaiter().GetResult()) 
    //            .NotEmpty().WithMessage(translationService.GetTranslationByKey("DefaultNotEmpty").GetAwaiter().GetResult());
    //    }
    }