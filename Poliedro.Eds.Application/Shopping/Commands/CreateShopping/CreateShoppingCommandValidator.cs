using FluentValidation;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
using Poliedro.Eds.Domain.Provider.DomainProvider;

namespace Poliedro.Eds.Application.Shopping.Shopping.CreateShopping;

public class CreateShoppingCommandValidator : AbstractValidator<CreateShoppingRequestDto>
{
    private readonly IProviderGetByIdService _providerService;

    public CreateShoppingCommandValidator(IRedisService redisService, IProviderGetByIdService providerService)
    {
        _providerService = providerService;

        RuleFor(x => x.Invoice)
          .MustAsync(async (dto, invoice, cancellationToken) =>
             {
                 var providerResult = await _providerService.GetByIdAsync(dto.IdProvider);
                 if (providerResult.IsSuccess && providerResult.Value != null)
                 {
                     if (string.Equals(providerResult.Value.Name, "otros", StringComparison.OrdinalIgnoreCase))
                     {
                         return true;
                     }
                 }
                 return !string.IsNullOrWhiteSpace(invoice);
             }).WithMessage("La factura es requerida cuando el proveedor no es 'otros'")
           .WithName("Invoice");

        RuleFor(x => x.Date).NotEmpty().WithMessage("La fecha es requerida").WithName("Date");

        RuleFor(x => x.IdProvider)
            .GreaterThan(0).WithMessage("El ID del proveedor debe ser mayor a 0")
            .WithName("IdProvider");

        RuleFor(x => x.IdCategory)
             .GreaterThan(0).WithMessage("El ID de la categoría debe ser mayor a 0");

        RuleForEach(x => x.ShoppingProducts)
            .Must(product => product.SellPrice > product.PurchasePrice)
            .WithMessage((_, product) => 
                $"El precio de venta (${product.SellPrice:N2}) debe ser mayor al precio de compra (${product.PurchasePrice:N2}). " +
                $"La operación ha sido rechazada para prevenir pérdidas.");
    }
}
