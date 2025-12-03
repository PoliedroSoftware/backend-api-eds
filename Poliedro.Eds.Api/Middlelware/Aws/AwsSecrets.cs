using System.Text.Json;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Poliedro.Eds.Application.Secrets.Aws.Dto;

namespace Poliedro.Eds.Api.Middlelware.aws;

public class AwsSecrets
{
    public static async Task<AwsSecretsDto> GetSecret(IConfiguration config)
    {
        string? secretName = config["AWS:SecretName"];
        string? region = config["AWS:Region"];

        if (string.IsNullOrEmpty(secretName))
            throw new ArgumentNullException(nameof(secretName), "AWS:SecretName configuration value is missing.");

        if (string.IsNullOrEmpty(region))
            throw new ArgumentNullException(nameof(region), "AWS:Region configuration value is missing.");

        IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

        GetSecretValueRequest request = new()
        {
            SecretId = secretName,
            VersionStage = config["AWS:AwsCurren"],
        };

        GetSecretValueResponse response;

        try
        {
            response = await client.GetSecretValueAsync(request);
        }
        catch
        {

            throw;
        }
        if (string.IsNullOrEmpty(response.SecretString))
            throw new InvalidOperationException("SecretString is null or empty.");

        var secretsDto = JsonSerializer.Deserialize<AwsSecretsDto>(response.SecretString,
              new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (secretsDto == null)
            throw new InvalidOperationException("Failed to deserialize AWS secret.");

        return secretsDto;

    }
}





