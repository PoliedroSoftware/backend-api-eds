using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Poliedro.Eds.Domain.Auth.DomainAuth;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Infraestructure.External.Keycloak.Services;

public class KeycloakAuthService(HttpClient _httpClient, IConfiguration _configuration) : IKeycloakAuthService
{
    public async Task<Result<KeycloakTokenResult, Error>> AuthenticateAsync(
      string username,
      string password)
    {
        try
        {
            var realm = _configuration["Keycloak:Realm"] ?? "AppEDS";
            var clientId = _configuration["Keycloak:ClientId"] ?? "application-eds";

            var content = new FormUrlEncodedContent(new[]
           {
             new KeyValuePair<string, string>("client_id", clientId),
             new KeyValuePair<string, string>("username", username),
             new KeyValuePair<string, string>("password", password),
             new KeyValuePair<string, string>("grant_type", "password")
            });

            var tokenUrl = $"{_configuration["Keycloak:KeycloakUri"]}/realms/{realm}/protocol/openid-connect/token";

            var response = await _httpClient.PostAsync(tokenUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var error = Error.CreateInstance("Keycloak", $"Authentication failed: {errorContent}", System.Net.HttpStatusCode.Unauthorized);
                return Result<KeycloakTokenResult, Error>.Failure(error);
            }

            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<KeycloakTokenResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (tokenResponse == null)
            {
                var error = Error.CreateInstance("Keycloak", "Failed to parse token response", System.Net.HttpStatusCode.InternalServerError);
                return Result<KeycloakTokenResult, Error>.Failure(error);
            }

            var result = new KeycloakTokenResult(
                    AccessToken: tokenResponse.AccessToken ?? string.Empty,
                    ExpiresIn: tokenResponse.ExpiresIn,
                    RefreshExpiresIn: tokenResponse.RefreshExpiresIn,
                    RefreshToken: tokenResponse.RefreshToken ?? string.Empty,
                    TokenType: tokenResponse.TokenType ?? "Bearer",
                    Scope: tokenResponse.Scope ?? string.Empty
         );

            return Result<KeycloakTokenResult, Error>.Success(result);
        }
        catch (Exception ex)
        {
            var error = Error.CreateInstance("Keycloak", $"Authentication error: {ex.Message}", System.Net.HttpStatusCode.InternalServerError);
            return Result<KeycloakTokenResult, Error>.Failure(error);
        }
    }

    private class KeycloakTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }
        
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        
        [JsonPropertyName("refresh_expires_in")]
        public int RefreshExpiresIn { get; set; }
  
        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
    
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }
        
        [JsonPropertyName("scope")]
        public string? Scope { get; set; }
    }
}
