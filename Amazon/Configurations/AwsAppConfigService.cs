using Amazon.AppConfigData;
using Amazon.AppConfigData.Model;
using System.Text.Json;

namespace Amazon.Configurations;

public class AwsAppConfigService(
    string applicationId,
    string environmentId,
    string configurationProfileId,
    RegionEndpoint region)
{
    private readonly AmazonAppConfigDataClient _client = new(region);

    public async Task<T> GetConfigurationAsync<T>()
    {
      
        var startResponse = await _client.StartConfigurationSessionAsync(new StartConfigurationSessionRequest
        {
            ApplicationIdentifier = applicationId,
            EnvironmentIdentifier = environmentId,
            ConfigurationProfileIdentifier = configurationProfileId
        });

        var getConfigResponse = await _client.GetLatestConfigurationAsync(new GetLatestConfigurationRequest
        {
            ConfigurationToken = startResponse.InitialConfigurationToken
        });

        using var ms = new MemoryStream(getConfigResponse.Configuration.ToArray());
        var config = await JsonSerializer.DeserializeAsync<T>(ms, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return config!;
    }
}
