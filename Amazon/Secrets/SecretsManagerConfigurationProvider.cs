using Amazon.SecretsManager;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Amazon.Secrets;

public class SecretsManagerConfigurationProvider(string secretName, string region) : ConfigurationProvider
{
    public override void Load()
    {
        var client = new AmazonSecretsManagerClient(Amazon.RegionEndpoint.GetBySystemName(region));
        var request = new SecretsManager.Model.GetSecretValueRequest { SecretId = secretName };
        var response = client.GetSecretValueAsync(request).GetAwaiter().GetResult();

        if (!string.IsNullOrEmpty(response.SecretString))
        {
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(response.SecretString);
            if (data != null)
            {
                foreach (var kvp in data)
                {
                    if (kvp.Value is JsonElement element && element.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var child in element.EnumerateObject())
                        {
                            Data[$"{kvp.Key}:{child.Name}"] = child.Value.GetString() ?? "";
                        }
                    }
                    else
                    {
                        Data[kvp.Key] = kvp.Value?.ToString() ?? "";
                    }
                }
            }
        }
    }
}

public class SecretsManagerConfigurationSource(string secretName, string region) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new SecretsManagerConfigurationProvider(secretName, region);
    }
}

public static class SecretsManagerConfigurationExtensions
{
    public static IConfigurationBuilder AddSecretsManager(this IConfigurationBuilder builder, string secretName, string region)
    {
        return builder.Add(new SecretsManagerConfigurationSource(secretName, region));
    }
}
