using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.BilligEds.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Domain.BilligEds.DomainService;

public class BilligEdsCreateService : IBilligEdsCreateDomainService
{
    private readonly HttpClient _httpClient;
    private const string EndpointPath = "api/v1/billigeds";

    public BilligEdsCreateService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    async Task<Result<VoidResult, Error>> IBilligEdsCreateDomainService.CreateBilligAsync(BillidEdsRequestEntity billigEdsRequest)
    {
        if (billigEdsRequest is null)
        {
            var bad = Error.CreateInstance("InvalidRequest", "Request body cannot be null.", System.Net.HttpStatusCode.BadRequest);
            return Result<VoidResult, Error>.Failure(bad);
        }

        try
        {
            var response = await _httpClient.PostAsJsonAsync(EndpointPath, billigEdsRequest);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var error = Error.CreateInstance(
                    "BilligEdsExternalApiError",
                    $"External API returned {(int)response.StatusCode}. Response: {content}",
                    response.StatusCode);

                return Result<VoidResult, Error>.Failure(error);
            }

            return Result<VoidResult, Error>.Success(VoidResult.Instance);
        }
        catch (HttpRequestException ex)
        {
            var error = Error.CreateInstance(
                "BilligEdsHttpRequestException",
                $"HttpRequestException when calling external billing service: {ex.Message}",
                System.Net.HttpStatusCode.InternalServerError);

            return Result<VoidResult, Error>.Failure(error);
        }
        catch (Exception ex)
        {
            var error = Error.CreateInstance(
                "BilligEdsUnexpectedException",
                $"Unexpected error when creating billing document: {ex.Message}",
                System.Net.HttpStatusCode.InternalServerError);

            return Result<VoidResult, Error>.Failure(error);
        }
    }
}
