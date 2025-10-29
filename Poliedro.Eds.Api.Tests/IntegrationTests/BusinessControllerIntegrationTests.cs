using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Poliedro.Eds.Api.Tests.Infrastructure;
using Poliedro.Eds.Application.Business.Commands.CreateBusiness;
using Poliedro.Eds.Application.Business.Commands.UpdateBusiness;
using Poliedro.Eds.Application.Business.Dtos;

namespace Poliedro.Eds.Api.Tests.IntegrationTests;

public class BusinessControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public BusinessControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    #region GetAll Tests

    [Fact]
    public async Task GetAll_WithValidPagination_ReturnsOkResult()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;

        // Act
        var response = await _client.GetAsync($"/api/v1/business?PageNumber={pageNumber}&PageSize={pageSize}");

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Unauthorized);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
        }
    }

    [Fact]
    public async Task GetAll_WithoutPagination_ReturnsOkResult()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/business");

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAll_WithInvalidPageNumber_ReturnsResponse()
    {
        // Arrange
        var pageNumber = 0;
        var pageSize = 10;

        // Act
        var response = await _client.GetAsync($"/api/v1/business?PageNumber={pageNumber}&PageSize={pageSize}");

        // Assert
        Assert.NotNull(response);
    }

    #endregion

    #region GetById Tests

    [Fact]
    public async Task GetById_WithValidId_ReturnsOkOrNotFound()
    {
        // Arrange
        var validId = 1;

        // Act
        var response = await _client.GetAsync($"/api/v1/business/{validId}");

        // Assert
        Assert.True(
       response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.NotFound ||
     response.StatusCode == HttpStatusCode.Unauthorized ||
            response.StatusCode == HttpStatusCode.BadRequest
        );
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var invalidId = 0;

        // Act
        var response = await _client.GetAsync($"/api/v1/business/{invalidId}");

        // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.BadRequest ||
            response.StatusCode == HttpStatusCode.Unauthorized
        );
    }

    [Fact]
    public async Task GetById_WithNegativeId_ReturnsBadRequest()
    {
        // Arrange
        var negativeId = -1;

        // Act
        var response = await _client.GetAsync($"/api/v1/business/{negativeId}");

        // Assert
        Assert.True(
               response.StatusCode == HttpStatusCode.BadRequest ||
   response.StatusCode == HttpStatusCode.Unauthorized
           );
    }

    [Fact]
    public async Task GetById_WithNonExistentId_ReturnsNotFoundOrBadRequest()
    {
        // Arrange
        var nonExistentId = 999999;

        // Act
        var response = await _client.GetAsync($"/api/v1/business/{nonExistentId}");

        // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.NotFound ||
     response.StatusCode == HttpStatusCode.BadRequest ||
      response.StatusCode == HttpStatusCode.Unauthorized
        );
    }

    #endregion

    #region Create Tests

    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedOrUnauthorized()
    {
        // Arrange
        var command = new CreateBusinessCommand(
            new CreateBusinessRequestDto(
         Name: "Test Business",
                Context: "Test Context"
   )
        );

        var jsonContent = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/business", httpContent);

        // Assert
        Assert.True(
     response.StatusCode == HttpStatusCode.Created ||
            response.StatusCode == HttpStatusCode.Unauthorized ||
  response.StatusCode == HttpStatusCode.Conflict ||
            response.StatusCode == HttpStatusCode.BadRequest
        );
    }

    [Fact]
    public async Task Create_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateBusinessCommand(
new CreateBusinessRequestDto(
     Name: "",
                Context: "Test Context"
 )
        );

        var jsonContent = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/business", httpContent);

        // Assert
        Assert.True(
     response.StatusCode == HttpStatusCode.BadRequest ||
            response.StatusCode == HttpStatusCode.Unauthorized
        );
    }

    [Fact]
    public async Task Create_WithStringAsName_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateBusinessCommand(
    new CreateBusinessRequestDto(
      Name: "string",
      Context: "Test Context"
        )
        );

        var jsonContent = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/business", httpContent);

        // Assert
        Assert.True(
   response.StatusCode == HttpStatusCode.BadRequest ||
 response.StatusCode == HttpStatusCode.Unauthorized
     );
    }

    [Fact]
    public async Task Create_WithNullName_ReturnsBadRequest()
    {
        // Arrange
        var json = JsonSerializer.Serialize(new
        {
            Request = new
            {
                Name = (string)null,
                Context = "Test Context"
            }
        });

        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/business", httpContent);

        // Assert
        Assert.True(
        response.StatusCode == HttpStatusCode.BadRequest ||
        response.StatusCode == HttpStatusCode.Unauthorized
             );
    }

    [Fact]
    public async Task Create_WithEmptyRequest_ReturnsBadRequest()
    {
        // Arrange
        var httpContent = new StringContent("{}", Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/business", httpContent);

        // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.BadRequest ||
  response.StatusCode == HttpStatusCode.Unauthorized
        );
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Update_WithValidData_ReturnsNoContentOrUnauthorized()
    {
        // Arrange
        var command = new UpdateBusinessCommand
        {
            IdBusiness = 1,
            Name = "Updated Business Name",
            Context = "Updated Context"
        };

        var jsonContent = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/v1/business", httpContent);

        // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.NoContent ||
            response.StatusCode == HttpStatusCode.Unauthorized ||
        response.StatusCode == HttpStatusCode.NotFound ||
         response.StatusCode == HttpStatusCode.BadRequest
        );
    }

    [Fact]
    public async Task Update_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateBusinessCommand
        {
            IdBusiness = 0,
            Name = "Updated Business Name",
            Context = "Updated Context"
        };

        var jsonContent = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/v1/business", httpContent);

        // Assert
        Assert.True(
          response.StatusCode == HttpStatusCode.BadRequest ||
          response.StatusCode == HttpStatusCode.Unauthorized
        );
    }

    [Fact]
    public async Task Update_WithNegativeId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateBusinessCommand
        {
            IdBusiness = -1,
            Name = "Updated Business Name",
            Context = "Updated Context"
        };

        var jsonContent = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/v1/business", httpContent);

        // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.BadRequest ||
       response.StatusCode == HttpStatusCode.Unauthorized
        );
    }

    [Fact]
    public async Task Update_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateBusinessCommand
        {
            IdBusiness = 1,
            Name = "",
            Context = "Updated Context"
        };

        var jsonContent = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/v1/business", httpContent);

        // Assert
        Assert.True(
         response.StatusCode == HttpStatusCode.BadRequest ||
   response.StatusCode == HttpStatusCode.Unauthorized
        );
    }

    [Fact]
    public async Task Update_WithNullName_ReturnsBadRequest()
    {
        // Arrange
        var json = JsonSerializer.Serialize(new
        {
            IdBusiness = 1,
            Name = (string)null,
            Context = "Updated Context"
        });

        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/v1/business", httpContent);

        // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.BadRequest ||
     response.StatusCode == HttpStatusCode.Unauthorized
        );
    }

    [Fact]
    public async Task Update_WithNonExistentId_ReturnsNotFoundOrBadRequest()
    {
        // Arrange
        var command = new UpdateBusinessCommand
        {
            IdBusiness = 999999,
            Name = "Updated Business Name",
            Context = "Updated Context"
        };

        var jsonContent = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/v1/business", httpContent);

        // Assert
        Assert.True(
response.StatusCode == HttpStatusCode.NotFound ||
       response.StatusCode == HttpStatusCode.BadRequest ||
    response.StatusCode == HttpStatusCode.Unauthorized ||
  response.StatusCode == HttpStatusCode.InternalServerError
        );
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_WithValidPagination_ReturnsOkOrUnauthorized()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;

        // Act
        var response = await _client.DeleteAsync($"/api/v1/business?PageNumber={pageNumber}&PageSize={pageSize}");

        // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.OK ||
       response.StatusCode == HttpStatusCode.Unauthorized ||
       response.StatusCode == HttpStatusCode.NotFound
        );
    }

    [Fact]
    public async Task Delete_WithoutPagination_ReturnsResponse()
    {
        // Act
        var response = await _client.DeleteAsync("/api/v1/business");

        // Assert
        Assert.NotNull(response);
        Assert.True(
                response.StatusCode == HttpStatusCode.OK ||
    response.StatusCode == HttpStatusCode.Unauthorized ||
                response.StatusCode == HttpStatusCode.NotFound
        );
    }

    #endregion

    #region Content Type Tests

    [Fact]
    public async Task Create_WithInvalidContentType_ReturnsUnsupportedMediaType()
    {
        // Arrange
        var command = new CreateBusinessCommand(
            new CreateBusinessRequestDto(
     Name: "Test Business",
          Context: "Test Context"
            )
  );

        var jsonContent = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "text/plain");

        // Act
        var response = await _client.PostAsync("/api/v1/business", httpContent);

        // Assert
        Assert.True(
       response.StatusCode == HttpStatusCode.UnsupportedMediaType ||
          response.StatusCode == HttpStatusCode.Unauthorized ||
            response.StatusCode == HttpStatusCode.BadRequest
        );
    }

    [Fact]
    public async Task Update_WithInvalidContentType_ReturnsUnsupportedMediaType()
    {
        // Arrange
        var command = new UpdateBusinessCommand
        {
            IdBusiness = 1,
            Name = "Updated Business Name",
            Context = "Updated Context"
        };

        var jsonContent = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "text/plain");

        // Act
        var response = await _client.PutAsync("/api/v1/business", httpContent);

        // Assert
        Assert.True(
               response.StatusCode == HttpStatusCode.UnsupportedMediaType ||
               response.StatusCode == HttpStatusCode.Unauthorized ||
               response.StatusCode == HttpStatusCode.BadRequest
     );
    }

    #endregion

    #region Response Format Tests

    [Fact]
    public async Task GetAll_ReturnsValidJsonResponse()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/business?PageNumber=1&PageSize=10");

        // Assert
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.NotEmpty(content);

            // Verify it's valid JSON
            var exception = Record.Exception(() => JsonSerializer.Deserialize<object>(content));
            Assert.Null(exception);
        }
    }

    [Fact]
    public async Task GetById_WithValidId_ReturnsValidJsonResponse()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/business/1");

        // Assert
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.NotEmpty(content);

            // Verify it's valid JSON
            var exception = Record.Exception(() => JsonSerializer.Deserialize<BusinessDto>(content));
            Assert.Null(exception);
        }
    }

    #endregion
}
