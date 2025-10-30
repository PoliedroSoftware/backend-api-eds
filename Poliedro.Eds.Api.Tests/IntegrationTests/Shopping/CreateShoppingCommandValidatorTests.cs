using FluentValidation.TestHelper;
using Moq;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
using Poliedro.Eds.Application.Shopping.Shopping.CreateShopping;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.Shopping;

public class CreateShoppingCommandValidatorTests
{
    private readonly CreateShoppingCommandValidator _validator;
    private readonly Mock<IRedisService> _redisServiceMock;

    public CreateShoppingCommandValidatorTests()
    {
        _redisServiceMock = new Mock<IRedisService>();
        _validator = new CreateShoppingCommandValidator(_redisServiceMock.Object);
    }

    [Fact]
    public void Validate_SellPriceGreaterThanPurchasePrice_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new CreateShoppingRequestDto(
            Invoice: "INV-001",
            Date: DateTime.Now,
            IdProvider: 1,
            IdCategory: 1,
            Amount: 10000,
            ShoppingProducts: new List<ShoppingProductRequestDto>
            {
                new ShoppingProductRequestDto(
                    IdProduct: 1,
                    Quantity: 100,
                    PurchasePrice: 16000,
                    SellPrice: 18000,
                    IdCompartment: 1
                )
            },
            ShoppingInventory: null
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_SellPriceLessThanPurchasePrice_ShouldHaveValidationError()
    {
        // Arrange
        var request = new CreateShoppingRequestDto(
            Invoice: "INV-001",
            Date: DateTime.Now,
            IdProvider: 1,
            IdCategory: 1,
            Amount: 10000,
            ShoppingProducts: new List<ShoppingProductRequestDto>
            {
                new ShoppingProductRequestDto(
                    IdProduct: 1,
                    Quantity: 100,
                    PurchasePrice: 16000,
                    SellPrice: 15000,
                    IdCompartment: 1
                )
            },
            ShoppingInventory: null
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShoppingProducts)
            .WithErrorMessage("El precio de venta ($15,000.00) debe ser mayor al precio de compra ($16,000.00). La operación ha sido rechazada para prevenir pérdidas.");
    }

    [Fact]
    public void Validate_SellPriceEqualToPurchasePrice_ShouldHaveValidationError()
    {
        // Arrange
        var request = new CreateShoppingRequestDto(
            Invoice: "INV-001",
            Date: DateTime.Now,
            IdProvider: 1,
            IdCategory: 1,
            Amount: 10000,
            ShoppingProducts: new List<ShoppingProductRequestDto>
            {
                new ShoppingProductRequestDto(
                    IdProduct: 1,
                    Quantity: 100,
                    PurchasePrice: 16000,
                    SellPrice: 16000,
                    IdCompartment: 1
                )
            },
            ShoppingInventory: null
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShoppingProducts)
            .WithErrorMessage("El precio de venta ($16,000.00) debe ser mayor al precio de compra ($16,000.00). La operación ha sido rechazada para prevenir pérdidas.");
    }

    [Fact]
    public void Validate_MultipleProducts_OnlyInvalidProductShouldHaveError()
    {
        // Arrange
        var request = new CreateShoppingRequestDto(
            Invoice: "INV-001",
            Date: DateTime.Now,
            IdProvider: 1,
            IdCategory: 1,
            Amount: 10000,
            ShoppingProducts: new List<ShoppingProductRequestDto>
            {
                new ShoppingProductRequestDto(
                    IdProduct: 1,
                    Quantity: 100,
                    PurchasePrice: 16000,
                    SellPrice: 18000,
                    IdCompartment: 1
                ),
                new ShoppingProductRequestDto(
                    IdProduct: 2,
                    Quantity: 200,
                    PurchasePrice: 12000,
                    SellPrice: 11000,
                    IdCompartment: 2
                )
            },
            ShoppingInventory: null
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShoppingProducts);
    }
}
