using FluentValidation.TestHelper;
using Moq;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Shopping.Commands.UpdateShoppingProduct;
using Poliedro.Eds.Application.ShoppingProduct.Commands.UpdateShoppingProduct;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.ShoppingProduct;

public class UpdateShoppingProductCommandValidatorTests
{
    private readonly UpdateShoppingProductCommandValidator _validator;
    private readonly Mock<IRedisService> _redisServiceMock;

    public UpdateShoppingProductCommandValidatorTests()
    {
        _redisServiceMock = new Mock<IRedisService>();
        _redisServiceMock.Setup(x => x.GetValueFromCacheAsync(It.IsAny<string>()))
            .ReturnsAsync("Validation error");
        _validator = new UpdateShoppingProductCommandValidator(_redisServiceMock.Object);
    }

    [Fact]
    public void Validate_SellPriceGreaterThanPurchasePrice_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new UpdateShoppingProductCommand(
            IdShoppingProduct: 1,
            IdShopping: 1,
            IdProduct: 1,
            Quantity: 100,
            PurchasePrice: 16000,
            SellPrice: 18000,
            IdCompartment: 1
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x);
    }

    [Fact]
    public void Validate_SellPriceLessThanPurchasePrice_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateShoppingProductCommand(
            IdShoppingProduct: 1,
            IdShopping: 1,
            IdProduct: 1,
            Quantity: 100,
            PurchasePrice: 16000,
            SellPrice: 15000,
            IdCompartment: 1
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("El precio de venta ($15,000.00) debe ser mayor al precio de compra ($16,000.00). La operación ha sido rechazada para prevenir pérdidas.");
    }

    [Fact]
    public void Validate_SellPriceEqualToPurchasePrice_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateShoppingProductCommand(
            IdShoppingProduct: 1,
            IdShopping: 1,
            IdProduct: 1,
            Quantity: 100,
            PurchasePrice: 16000,
            SellPrice: 16000,
            IdCompartment: 1
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("El precio de venta ($16,000.00) debe ser mayor al precio de compra ($16,000.00). La operación ha sido rechazada para prevenir pérdidas.");
    }

    [Fact]
    public void Validate_SellPriceZeroAndPurchaseZero_ShouldHaveValidationError()
    {
        // Arrange - Edge case where both are zero
        var command = new UpdateShoppingProductCommand(
            IdShoppingProduct: 1,
            IdShopping: 1,
            IdProduct: 1,
            Quantity: 100,
            PurchasePrice: 0,
            SellPrice: 0,
            IdCompartment: 1
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("El precio de venta ($0.00) debe ser mayor al precio de compra ($0.00). La operación ha sido rechazada para prevenir pérdidas.");
    }
}
