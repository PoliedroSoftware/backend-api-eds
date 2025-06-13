namespace Poliedro.Eds.Application.Secrets.Aws.Dto;
using System.Text.Json.Serialization;

public record AwsSecretsDto
{
    [JsonPropertyName("aws_access_key_id")]
    public string AwsAccessKeyId { get; init; } = string.Empty;

    [JsonPropertyName("aws_secret_access_key")]
    public string AwsSecretAccessKey { get; init; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; init; } = string.Empty;
}


