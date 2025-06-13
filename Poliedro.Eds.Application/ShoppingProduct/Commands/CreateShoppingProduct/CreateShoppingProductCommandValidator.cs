using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;

namespace Poliedro.Eds.Application.ShoppingProduct.Shopping.CreateShoppingProduct;

public class CreateShoppingProductCommandValidator : AbstractValidator<CreateShoppingProductRequestDto>
{
    private readonly ICompartimentGetByIdCompartiment _compartimentService;
    private readonly IProductCompartimentGetByCompartmentId _productCompartimentService;

    public CreateShoppingProductCommandValidator(
        ICompartimentGetByIdCompartiment compartimentService,
        IRedisService redisService,
        IProductCompartimentGetByCompartmentId productCompartimentService)
    {
        _compartimentService = compartimentService;
        _productCompartimentService = productCompartimentService;

        RuleFor(x => x)
            .MustAsync(async (dto, cancellation) =>
            {
                var compartimentResult = await _compartimentService.GetByIdAsync(dto.IdCompartment);
                if (!compartimentResult.IsSuccess || compartimentResult.Value == null)
                    return false;

                var compartiment = compartimentResult.Value;
                return dto.Quantity + compartiment.Stock < compartiment.Operative;
            })
            .WithMessage((dto, context) =>
            {
                var compartimentResult = _compartimentService.GetByIdAsync(dto.IdCompartment).Result;
                if (compartimentResult.IsSuccess && compartimentResult.Value != null)
                {
                    var compartiment = compartimentResult.Value;
                    var suma = dto.Quantity + compartiment.Stock;
                    return $"Hay {compartiment.Stock} gls en el Tanque, esta compra de {dto.Quantity} gls supera la capacidad operativa del Tanque ({compartiment.Operative} gls.)";
                }
                return "La suma de la compra más el stock actual supera la capacidad operativa del compartimento.";
            });

        RuleFor(x => x)
            .MustAsync(async (dto, cancellation) =>
            {
                var idProduct = await _productCompartimentService.GetProductIdByCompartmentIdAsync(dto.IdCompartment);
                return idProduct.HasValue && idProduct.Value == dto.IdProduct;
            })
            .WithMessage("El producto comprado no coincide con el producto asignado a este tanque");

    }

    //public CreateShoppingProductCommandValidator(ITranslationService translationService)
    //{
    //    RuleFor(x => x.IdShopping)
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdShoppingGreaterThan").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdShoppingNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.IdProduct)
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdProductGreaterThan").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdProductNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.Quantity)
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("QuantityGreaterThan").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("QuantityNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.Price)
    //        .GreaterThanOrEqualTo(0).WithMessage(translationService.GetTranslationByKey("PriceGreaterThanOrEqualTo").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("PriceNotEmpty").GetAwaiter().GetResult());

    //    RuleFor(x => x.IdCompartment)
    //        .GreaterThan(0).WithMessage(translationService.GetTranslationByKey("IdCompartmentGreaterThan").GetAwaiter().GetResult())
    //        .NotEmpty().WithMessage(translationService.GetTranslationByKey("IdCompartmentNotEmpty").GetAwaiter().GetResult());
    //}
}