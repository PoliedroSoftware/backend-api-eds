using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Poliedro.Eds.Application.Secrets.Aws.Dto;
using System.Text.Json;

namespace Poliedro.Eds.Api.Middlelware.aws;

public class AwsSecrets(IConfiguration config)
{
    public static async Task<AwsSecretsDto> GetSecret(IConfiguration config)
    {
        string secretName = config["AWS:SecretName"];
        string region = config["AWS:Region"];

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
        catch (Exception e)
        {

            throw e;
        }
        return JsonSerializer.Deserialize<AwsSecretsDto>(response.SecretString,
              new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    }
}



