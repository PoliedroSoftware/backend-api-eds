using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;

namespace Poliedro.Eds.Infraestructure.External.Keycloak.Services
{
    public class KeycloakService : IKeycloakUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IIslanderCreateIslander _islanderDomainIslander;

        public KeycloakService(
            HttpClient httpClient,
            IConfiguration configuration,
            IIslanderCreateIslander islanderDomainIslander)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _islanderDomainIslander = islanderDomainIslander;
        }

        public async Task<Result<VoidResult, Error>> CreateUserAsync(IslanderEntity islander, string plainPassword, string? nameClaimToken)
        {
            try
            {
                var token = await GetAdminTokenAsync();

                if (token == null)
                    return Error.BadRequest("Keycloak", "Failed to get token");

                var payload = new
                {
                    username = islander.Name,
                    email = islander.Email,
                    firstName = islander.FirstName,
                    lastName = islander.LastName,
                    enabled = true,
                    credentials = new[]
                    {
                        new {
                            type = "password",
                            value = plainPassword,
                            temporary = false
                        }
                    },
                    attributes = new
                    {
                        id_eds = new[] { islander.IdEds.ToString() },
                    },
                };

                Console.WriteLine(payload);

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var realm = _configuration["Keycloak:Realm"];
                var url = $"{_configuration["Keycloak:KeycloakUri"]}/admin/realms/{realm}/users";

                Console.WriteLine(url);


                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Error.Conflict("Keycloak", $"Error creating user: {errorMsg}");
                }


                var location = response.Headers.Location?.ToString();

                var userId = location?.Split('/').Last();

                var resetPasswordPayload = new
                {
                    type = "password",
                    value = plainPassword,
                    temporary = false
                };

                var resetPasswordContent = new StringContent(JsonSerializer.Serialize(resetPasswordPayload), Encoding.UTF8, "application/json");

                var resetPasswordUrl = $"{_configuration["Keycloak:KeycloakUri"]}/admin/realms/{realm}/users/{userId}/reset-password";

                var resetPasswordResponse = await _httpClient.PutAsync(resetPasswordUrl, resetPasswordContent);

                if (!resetPasswordResponse.IsSuccessStatusCode)
                {
                    var pwdError = await resetPasswordResponse.Content.ReadAsStringAsync();
                    return Error.Conflict("Keycloak", $"Error setting user password: {pwdError}");

                }


                var groupId = _configuration["Keycloak:DefaultGroupId"];

                var subgroupsUrl = $"{_configuration["Keycloak:KeycloakUri"]}/admin/realms/{realm}/groups/{groupId}/children";
                var groupResponse = await _httpClient.GetAsync(subgroupsUrl);

                if (!groupResponse.IsSuccessStatusCode)
                {
                    var errorText = await groupResponse.Content.ReadAsStringAsync();
                    return Error.Conflict("Keycloak", $"Error getting subgroups: {errorText}");
                }

                var groupContent = await groupResponse.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(groupContent);


                var subGroups = jsonDoc.RootElement.EnumerateArray();

                string? subGroupId = null;

                string Normalize(string? input) =>
                    string.Join(" ", (input ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries));


                foreach (var subgroup in subGroups)
                {
                    if (Normalize(subgroup.GetProperty("name").GetString()) == Normalize(nameClaimToken))
                    {
                        subGroupId = subgroup.GetProperty("id").GetString();
                        break;
                    }
                }

                if (subGroupId == null)
                {
                    Console.WriteLine("Keycloak", $"No se encontró subgrupo con nombre: {nameClaimToken}");

                    return Error.Conflict("Keycloak", $"No se encontró subgrupo con nombre: {nameClaimToken}");
                }

                var assignUrl = $"{_configuration["Keycloak:KeycloakUri"]}/admin/realms/{realm}/users/{userId}/groups/{subGroupId}";
                var assignResponse = await _httpClient.PutAsync(assignUrl, null);

                if (!assignResponse.IsSuccessStatusCode)
                {
                    var errorText = await assignResponse.Content.ReadAsStringAsync();
                    Console.WriteLine(errorText);

                    return Error.Conflict("Keycloak", $"Error assigning user to sub-group: {errorText}");
                }
                if (assignResponse.IsSuccessStatusCode)
                {
                    await _islanderDomainIslander.CreateAsync(islander);
                }

                return VoidResult.Instance;
            }
            catch (Exception ex)
            {
                return Error.Internal("Keycloak", ex.Message);
            }
        }

        private async Task<string?> GetAdminTokenAsync()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id","eds-backend-service"),
                new KeyValuePair<string, string>("client_secret", "sZxDY46BQ9IasIZc0Tk01LSGkjb4Dytr")
            });

            var tokenUrl = $"{_configuration["Keycloak:KeycloakUri"]}/realms/AppEDS/protocol/openid-connect/token";


            var response = await _httpClient.PostAsync(tokenUrl, content);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("access_token").GetString();
        }
    }
}
