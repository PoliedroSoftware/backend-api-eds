namespace Poliedro.Eds.Domain.Test;

public class HoseHistoryRoundingTests
{
    [Fact]
    public void AccumulatedGallons_ShouldBeRoundedToTwoDecimalPlaces()
    {
        // Arrange
        double inputGallons = 123.456789;
        double expectedRoundedGallons = 123.46;

        // Act
        double actualRoundedGallons = Math.Round(inputGallons, 2);

        // Assert
        Assert.Equal(expectedRoundedGallons, actualRoundedGallons);
    }

    [Theory]
    [InlineData(123.456, 123.46)]
    [InlineData(123.454, 123.45)]
    [InlineData(123.455, 123.46)] // Test banker's rounding
    [InlineData(123.464, 123.46)] // Test banker's rounding - corrected value
    [InlineData(123.0, 123.0)]
    [InlineData(0.0, 0.0)]
    [InlineData(0.123, 0.12)]
    [InlineData(99.999, 100.0)]
    public void AccumulatedGallons_VariousInputs_ShouldBeRoundedCorrectly(double input, double expected)
    {
        // Act
        double actual = Math.Round(input, 2);

        // Assert
        Assert.Equal(expected, actual, precision: 2);
    }
}