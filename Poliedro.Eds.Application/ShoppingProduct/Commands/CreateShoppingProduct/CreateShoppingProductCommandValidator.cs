using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;

namespace Poliedro.Eds.Application.ShoppingProduct.Shopping.CreateShoppingProduct;

public class CreateShoppingProductCommandValidator : AbstractValidator<CreateShoppingProductRequestDto>
{
    private readonly ICompartimentGetByIdService _compartimentService;

    public CreateShoppingProductCommandValidator(
        ICompartimentGetByIdService compartimentService,
        IRedisService redisService)
    {
        _compartimentService = compartimentService;

        //RuleFor(x => x)
        //    .MustAsync(async (dto, cancellation) =>
        //    {
        //        var compartimentResult = await _compartimentService.GetByIdAsync(dto.IdCompartment);
        //        if (!compartimentResult.IsSuccess || compartimentResult.Value == null)
        //            return false;

        //        var compartiment = compartimentResult.Value;
        //        return dto.Quantity + compartiment.Stock < compartiment.Operative;
        //    })
        //    .WithMessage((dto, context) =>
        //    {
        //        var compartimentResult = _compartimentService.GetByIdAsync(dto.IdCompartment).Result;
        //        if (compartimentResult.IsSuccess && compartimentResult.Value != null)
        //        {
        //            var compartiment = compartimentResult.Value;
        //            var suma = dto.Quantity + compartiment.Stock;
        //            return $"Hay {compartiment.Stock} gls en el Tanque, esta compra de {dto.Quantity} gls supera la capacidad operativa del Tanque ({compartiment.Operative} gls.)";
        //        }
        //        return "La suma de la compra más el stock actual supera la capacidad operativa del compartimento.";
        //    });

        //RuleFor(x => x)
        //    .MustAsync(async (dto, cancellation) =>
        //    {
        //        var idProduct = await _productCompartimentService.GetProductIdByCompartmentIdAsync(dto.IdCompartment);
        //        return idProduct.HasValue && idProduct.Value == dto.IdProduct;
        //    })
        //    .WithMessage("El producto comprado no coincide con el producto asignado a este tanque");

    }

    public CreateShoppingProductCommandValidator(IRedisService redisService)
    {
        RuleFor(x => x.IdShopping)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdShoppingGreaterThan").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdShoppingNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.IdProduct)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdProductGreaterThan").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdProductNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("QuantityGreaterThan").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("QuantityNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("PriceGreaterThanOrEqualTo").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("PriceNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.IdCompartment)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdCompartmentGreaterThan").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdCompartmentNotEmpty").GetAwaiter().GetResult());
    }
}
