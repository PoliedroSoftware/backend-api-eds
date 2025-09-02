using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Product.DomainProduct;

namespace Poliedro.Eds.Application.ShoppingProduct.Shopping.CreateShoppingProduct;

public class CreateShoppingProductCommandValidator : AbstractValidator<CreateShoppingProductRequestDto>
{
    private readonly ICompartimentGetByIdService _compartimentService;
    private readonly IProductGetByIdProduct _productService;

    public CreateShoppingProductCommandValidator(
        ICompartimentGetByIdService compartimentService,
        IRedisService redisService,
        IProductGetByIdProduct productService)
    {
        _compartimentService = compartimentService;
        _productService = productService;

        //RuleFor(x => x)
        //    .MustAsync(async (dto, cancellation) =>
        //    {
        //        var compartimentResult = await _compartimentService.GetByIdAsync(dto.IdCompartment);
        //        if (!compartimentResult.IsSuccess || compartimentResult.Value == null)
        //            return false;

                var productResult = await _productService.GetByIdAsync(dto.IdProduct);
                if (!productResult.IsSuccess || productResult.Value == null)
                    return false;

                var compartiment = compartimentResult.Value;
                var product = productResult.Value;
                
                // Verificar que el producto corresponda al compartimento
                if (compartiment.IdProduct != dto.IdProduct)
                    return false;
                
                // Verificar que la suma del stock actual del producto más la cantidad a comprar no supere la capacidad operativa del compartimento
                return (product.Stock + dto.Quantity) <= compartiment.Operative;
            })
            .WithMessage((dto, context) =>
            {
                // Sincronizar la llamada para evitar problemas con async en WithMessage
                var compartimentResult = _compartimentService.GetByIdAsync(dto.IdCompartment).GetAwaiter().GetResult();
                var productResult = _productService.GetByIdAsync(dto.IdProduct).GetAwaiter().GetResult();
                
                if (compartimentResult.IsSuccess && compartimentResult.Value != null && 
                    productResult.IsSuccess && productResult.Value != null)
                {
                    var compartiment = compartimentResult.Value;
                    var product = productResult.Value;
                    
                    // Verificar si es un problema de producto incorrecto
                    if (compartiment.IdProduct != dto.IdProduct)
                    {
                        return $"El producto {product.Name} no corresponde al compartimento {compartiment.Number}. El compartimento está asignado al producto ID {compartiment.IdProduct}.";
                    }
                    
                    // Problema de capacidad - el stock ahora está en el producto, no en el compartimento
                    var suma = product.Stock + dto.Quantity;
                    return $"Hay {product.Stock} gls de {product.Name} en stock, esta compra de {dto.Quantity} gls supera la capacidad operativa del Tanque {compartiment.Number} ({compartiment.Operative} gls.)";
                }
                return "Error en la validación del producto y compartimento.";
            });

        RuleFor(x => x.IdProduct)
            .MustAsync(async (idProduct, cancellation) =>
            {
                var productResult = await _productService.GetByIdAsync(idProduct);
                return productResult.IsSuccess && productResult.Value != null;
            })
            .WithMessage("El producto especificado no existe.");

        RuleFor(x => x.IdCompartment)
            .MustAsync(async (idCompartment, cancellation) =>
            {
                var compartimentResult = await _compartimentService.GetByIdAsync(idCompartment);
                return compartimentResult.IsSuccess && compartimentResult.Value != null;
            })
            .WithMessage("El compartimento especificado no existe.");
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

        RuleFor(x => x.PurchasePrice)
            .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("PurchasePriceGreaterThanOrEqualTo").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("PurchasePriceNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.SellPrice)
            .GreaterThanOrEqualTo(0).WithMessage(redisService.GetValueFromCacheAsync("SellPriceGreaterThanOrEqualTo").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("SellPriceNotEmpty").GetAwaiter().GetResult());

        RuleFor(x => x.IdCompartment)
            .GreaterThan(0).WithMessage(redisService.GetValueFromCacheAsync("IdCompartmentGreaterThan").GetAwaiter().GetResult())
            .NotEmpty().WithMessage(redisService.GetValueFromCacheAsync("IdCompartmentNotEmpty").GetAwaiter().GetResult());
    }
}
