using Microsoft.AspNetCore.Http;
using Moq;
using Poliedro.Eds.Application.FileUploadS3.Command;
using Poliedro.Eds.Domain.FileUploadS3.Ports;

namespace Poliedro.Eds.Api.Tests;

public class ImageDescriptionTests
{
    [Fact]
    public async Task DescribeImageHandler_ShouldCallImageDescriptionService()
    {
        // Arrange
        var mockImageDescriptionService = new Mock<IImageDescriptionService>();
        var mockFile = new Mock<IFormFile>();
        
        mockFile.Setup(f => f.ContentType).Returns("image/jpeg");
        mockFile.Setup(f => f.Length).Returns(1000);
        
        mockImageDescriptionService
            .Setup(s => s.DescribeImageAsync(It.IsAny<IFormFile>()))
            .ReturnsAsync("Test image description");

        var handler = new DescribeImageHandler(mockImageDescriptionService.Object);
        var command = new DescribeImageCommand(mockFile.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Test image description", result);
        mockImageDescriptionService.Verify(s => s.DescribeImageAsync(mockFile.Object), Times.Once);
    }

    [Fact]
    public void DescribeImageCommand_ShouldCreateWithImage()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.ContentType).Returns("image/png");

        // Act
        var command = new DescribeImageCommand(mockFile.Object);

        // Assert
        Assert.NotNull(command);
        Assert.Equal(mockFile.Object, command.Image);
    }
}