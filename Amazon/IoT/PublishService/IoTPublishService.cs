using System.Text;
using System.Text.Json;
using Amazon.IotData;
using Amazon.IotData.Model;
using global::Amazon;
using global::Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.IoT.Models;
using Poliedro.Eds.Domain.IoT.Ports;

namespace Amazon.IoT.PublishService;

public class IoTPublishService : IIoTPublishService
{
    private readonly IAmazonIotData _iotDataClient;
    private readonly ILogger<IoTPublishService> _logger;
    private readonly string _endpoint;
    private readonly bool _isInitialized;

    public IoTPublishService(IConfiguration configuration, ILogger<IoTPublishService> logger)
    {
        _logger = logger;
        _isInitialized = false;

        try
        {
            _endpoint = configuration["AWS:IoT:Endpoint"] 
                ?? throw new ArgumentNullException("AWS:IoT:Endpoint configuration is missing");
            
            var region = configuration["AWS:Region"] 
                ?? throw new ArgumentNullException("AWS:Region configuration is missing");

            _logger.LogInformation("Initializing AWS IoT Data client...");
            _logger.LogInformation("Endpoint: {Endpoint}", _endpoint);
            _logger.LogInformation("Region: {Region}", region);

            // Intentar obtener credenciales y validar
            AWSCredentials credentials;
            try
            {
                credentials = FallbackCredentialsFactory.GetCredentials();
                
                // Obtener credenciales inmutables para validar
                var immutableCredentials = credentials.GetCredentials();
                
                if (immutableCredentials == null)
                {
                    throw new AmazonClientException("Unable to load AWS credentials");
                }

                _logger.LogInformation("AWS credentials loaded successfully");
                _logger.LogDebug("Access Key ID: {AccessKeyId}", 
                    string.IsNullOrEmpty(immutableCredentials.AccessKey) 
                        ? "NOT SET" 
                        : immutableCredentials.AccessKey.Substring(0, Math.Min(4, immutableCredentials.AccessKey.Length)) + "...");
            }
            catch (Exception credEx)
            {
                _logger.LogError(credEx, "Failed to load AWS credentials. Make sure credentials are configured.");
                _logger.LogError("Please configure AWS credentials using one of these methods:");
                _logger.LogError("1. Environment variables: AWS_ACCESS_KEY_ID and AWS_SECRET_ACCESS_KEY");
                _logger.LogError("2. Credentials file: ~/.aws/credentials");
                _logger.LogError("3. IAM Role (if running on AWS)");
                throw new InvalidOperationException("AWS credentials not configured", credEx);
            }
            
            // Para AWS IoT Data, solo se debe especificar el ServiceURL
            var clientConfig = new AmazonIotDataConfig
            {
                ServiceURL = $"https://{_endpoint}",
                Timeout = TimeSpan.FromSeconds(30),
                MaxErrorRetry = 3
            };

            // Crear el cliente de AWS IoT Data
            _iotDataClient = new AmazonIotDataClient(credentials, clientConfig);
            
            _isInitialized = true;
            _logger.LogInformation(
                "AWS IoT Data client initialized successfully with endpoint: {Endpoint} in region: {Region}", 
                _endpoint, 
                region);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize AWS IoT Data client");
            _logger.LogError("Initialization failed. The service will not be able to publish messages.");
            throw;
        }
    }

    public async Task<bool> PublishMessageAsync(IoTMessage message, string topic)
    {
        if (!_isInitialized)
        {
            _logger.LogError("IoT service is not initialized. Cannot publish message.");
            return false;
        }

        try
        {
            _logger.LogInformation("Attempting to publish message to IoT topic: {Topic}", topic);
            _logger.LogDebug("Endpoint: {Endpoint}", _endpoint);

            // Serializar el mensaje a JSON
            var json = JsonSerializer.Serialize(message, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });

            _logger.LogDebug("Message payload: {Payload}", json);
            _logger.LogDebug("Payload size: {Size} bytes", Encoding.UTF8.GetByteCount(json));

            // Crear la petición de publicación
            var publishRequest = new PublishRequest
            {
                Topic = topic,
                Qos = 1, // QoS 1: At least once delivery
                Payload = new MemoryStream(Encoding.UTF8.GetBytes(json))
            };

            // Publicar el mensaje
            _logger.LogDebug("Sending publish request to AWS IoT Core...");
            var response = await _iotDataClient.PublishAsync(publishRequest);

            _logger.LogDebug("Response received. Status: {StatusCode}", response.HttpStatusCode);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                _logger.LogInformation("Message published successfully to topic: {Topic}", topic);
                return true;
            }
            else
            {
                _logger.LogWarning(
                    "Failed to publish message to topic: {Topic}. Status: {StatusCode}", 
                    topic, 
                    response.HttpStatusCode);
                return false;
            }
        }
        catch (AmazonIotDataException iotEx)
        {
            _logger.LogError(
                iotEx, 
                "AWS IoT Data error publishing message to topic: {Topic}. Error Code: {ErrorCode}, Status: {StatusCode}, Message: {Message}", 
                topic, 
                iotEx.ErrorCode, 
                iotEx.StatusCode,
                iotEx.Message);
            
            // Proporcionar sugerencias basadas en el error
            LogErrorSuggestions(iotEx.ErrorCode, (int)iotEx.StatusCode);
            return false;
        }
        catch (AmazonServiceException awsEx)
        {
            _logger.LogError(
                awsEx, 
                "AWS Service error publishing message to topic: {Topic}. Error Code: {ErrorCode}, Message: {Message}", 
                topic, 
                awsEx.ErrorCode,
                awsEx.Message);
            
            LogErrorSuggestions(awsEx.ErrorCode, (int)awsEx.StatusCode);
            return false;
        }
        catch (System.Net.Http.HttpRequestException httpEx)
        {
            _logger.LogError(
                httpEx, 
                "HTTP request error publishing message to topic: {Topic}. This usually indicates network or DNS issues.", 
                topic);
            _logger.LogError("Check:");
            _logger.LogError("- Internet connection");
            _logger.LogError("- Endpoint is correct: {Endpoint}", _endpoint);
            _logger.LogError("- DNS resolution for {Endpoint}", _endpoint);
            _logger.LogError("- Firewall/proxy settings");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error publishing message to AWS IoT Core topic: {Topic}", topic);
            return false;
        }
    }

    private void LogErrorSuggestions(string errorCode, int statusCode)
    {
        switch (errorCode)
        {
            case "UnauthorizedException":
            case "AccessDeniedException":
                _logger.LogError("Authentication/Authorization Error:");
                _logger.LogError("- Check that AWS credentials are correctly configured");
                _logger.LogError("- Verify credentials have not expired");
                _logger.LogError("- Ensure IAM user/role has iot:Publish permission");
                break;
                
            case "ForbiddenException":
                _logger.LogError("Permission Error:");
                _logger.LogError("- IAM policy does not allow publishing to topic: {Topic}");
                _logger.LogError("- Add iot:Publish permission for this topic in IAM policy");
                break;
                
            case "InvalidRequestException":
                _logger.LogError("Invalid Request:");
                _logger.LogError("- Check that topic name is valid (no special characters or spaces)");
                _logger.LogError("- Verify message format is correct");
                break;
                
            case "ThrottlingException":
                _logger.LogError("Rate Limit Exceeded:");
                _logger.LogError("- Too many requests. Implement backoff and retry logic");
                break;
        }
    }
}
