using FluentValidation.TestHelper;
using Moq;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
using Poliedro.Eds.Application.ShoppingProduct.Shopping.CreateShoppingProduct;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.ShoppingProduct;

public class CreateShoppingProductCommandValidatorTests
{
    private readonly CreateShoppingProductCommandValidator _validator;
    private readonly Mock<IRedisService> _redisServiceMock;

    public CreateShoppingProductCommandValidatorTests()
    {
        _redisServiceMock = new Mock<IRedisService>();
        _redisServiceMock.Setup(x => x.GetValueFromCacheAsync(It.IsAny<string>()))
            .ReturnsAsync("Validation error");
        _validator = new CreateShoppingProductCommandValidator(_redisServiceMock.Object);
    }

    [Fact]
    public void Validate_SellPriceGreaterThanPurchasePrice_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new CreateShoppingProductRequestDto(
            IdShopping: 157,
            IdProduct: 24,
            Quantity: 100,
            PurchasePrice: 16000,
            SellPrice: 18000,
            IdCompartment: 72
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x);
    }

    [Fact]
    public void Validate_SellPriceLessThanPurchasePrice_ShouldHaveValidationError()
    {
        // Arrange
        var request = new CreateShoppingProductRequestDto(
            IdShopping: 157,
            IdProduct: 24,
            Quantity: 100,
            PurchasePrice: 16000,
            SellPrice: 15000,
            IdCompartment: 72
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("El precio de venta ($15,000.00) debe ser mayor al precio de compra ($16,000.00). La operación ha sido rechazada para prevenir pérdidas.");
    }

    [Fact]
    public void Validate_SellPriceEqualToPurchasePrice_ShouldHaveValidationError()
    {
        // Arrange
        var request = new CreateShoppingProductRequestDto(
            IdShopping: 157,
            IdProduct: 24,
            Quantity: 100,
            PurchasePrice: 16000,
            SellPrice: 16000,
            IdCompartment: 72
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("El precio de venta ($16,000.00) debe ser mayor al precio de compra ($16,000.00). La operación ha sido rechazada para prevenir pérdidas.");
    }

    [Fact]
    public void Validate_SellPriceZeroAndPurchaseZero_ShouldHaveValidationError()
    {
        // Arrange - Edge case where both are zero
        var request = new CreateShoppingProductRequestDto(
            IdShopping: 157,
            IdProduct: 24,
            Quantity: 100,
            PurchasePrice: 0,
            SellPrice: 0,
            IdCompartment: 72
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("El precio de venta ($0.00) debe ser mayor al precio de compra ($0.00). La operación ha sido rechazada para prevenir pérdidas.");
    }
}
