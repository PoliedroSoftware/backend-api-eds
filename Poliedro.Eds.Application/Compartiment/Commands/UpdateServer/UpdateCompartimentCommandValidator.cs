using FluentValidation;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Poliedro.Eds.Application.Ports.Translations;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Poliedro.Eds.Application.Compartiment.Commands.UpdateCompartiment
{
    public class UpdateCompartimentCommandValidator : AbstractValidator<UpdateCompartimentCommand>
    {
        //public UpdateCompartimentCommandValidator(ITranslationService translationService)
        //{
        //    RuleFor(x => x.IdCompartment)
        //   .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdCompartmentGreaterThan").GetAwaiter().GetResult())
        //   .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdCompartmentNotEmpty").GetAwaiter().GetResult()); 

        //    RuleFor(x => x.Number)
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("NumberGreaterThan").GetAwaiter().GetResult()); 

        //    RuleFor(x => x.Nominal)
        //        .GreaterThanOrEqualTo(0).WithMessage(translationService.GetTranslationByKey("NominalGreaterThanOrEqualTo").GetAwaiter().GetResult());
        //    RuleFor(x => x.Operative)
        //        .GreaterThanOrEqualTo(0).WithMessage(translationService.GetTranslationByKey("OperativeGreaterThanOrEqualTo").GetAwaiter().GetResult()) 
        //        .LessThanOrEqualTo(x => x.Stock).WithMessage(translationService.GetTranslationByKey("OperativeGreaterThanOrEqualTo").GetAwaiter().GetResult());

        //    RuleFor(x => x.Stock)
        //        .GreaterThanOrEqualTo(0).WithMessage(translationService.GetTranslationByKey("StockGreaterThanOrEqualTo").GetAwaiter().GetResult());

        //    RuleFor(x => x.Height)
        //        .GreaterThanOrEqualTo(0).WithMessage(translationService.GetTranslationByKey("HeightGreaterThanOrEqualTo").GetAwaiter().GetResult());

        //    RuleFor(x => x.IdTank)
        //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdTankGreaterThan").GetAwaiter().GetResult())  
        //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdTankNotEmpty").GetAwaiter().GetResult()); 
        //}
    }
}
