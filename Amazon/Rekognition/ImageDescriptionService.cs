using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Poliedro.Eds.Domain.FileUploadS3.Ports;

namespace Amazon.Rekognition;

public class ImageDescriptionService : IImageDescriptionService
{
    private readonly IAmazonRekognition _rekognitionClient;

    public ImageDescriptionService(IConfiguration configuration)
    {
        var region = configuration["AWS:Region"] ?? throw new ArgumentNullException("Region configuration is missing");
        var regionEndpoint = RegionEndpoint.GetBySystemName(region);
        _rekognitionClient = new AmazonRekognitionClient(regionEndpoint);
    }

    public async Task<string> DescribeImageAsync(IFormFile image)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var detectLabelsRequest = new DetectLabelsRequest
            {
                Image = new Image
                {
                    Bytes = memoryStream
                },
                MaxLabels = 10,
                MinConfidence = 75F
            };

            var response = await _rekognitionClient.DetectLabelsAsync(detectLabelsRequest);
            
            var labels = response.Labels
                .Where(label => label.Confidence >= 75)
                .Select(label => $"{label.Name} ({label.Confidence:F1}%)")
                .Take(5);

            return labels.Any() 
                ? $"This image contains: {string.Join(", ", labels)}"
                : "Unable to identify clear objects in this image.";
        }
        catch (Exception ex)
        {
            throw new Exception($"Error analyzing image with AWS Rekognition: {ex.Message}", ex);
        }
    }

    public async Task<string> DescribeImageAsync(string imageUrl)
    {
        try
        {
            var detectLabelsRequest = new DetectLabelsRequest
            {
                Image = new Image
                {
                    S3Object = new S3Object
                    {
                        Bucket = ExtractBucketFromUrl(imageUrl),
                        Name = ExtractKeyFromUrl(imageUrl)
                    }
                },
                MaxLabels = 10,
                MinConfidence = 75F
            };

            var response = await _rekognitionClient.DetectLabelsAsync(detectLabelsRequest);
            
            var labels = response.Labels
                .Where(label => label.Confidence >= 75)
                .Select(label => $"{label.Name} ({label.Confidence:F1}%)")
                .Take(5);

            return labels.Any() 
                ? $"This image contains: {string.Join(", ", labels)}"
                : "Unable to identify clear objects in this image.";
        }
        catch (Exception ex)
        {
            throw new Exception($"Error analyzing image with AWS Rekognition: {ex.Message}", ex);
        }
    }

    private static string ExtractBucketFromUrl(string s3Url)
    {
        // Expected format: https://s3.region.amazonaws.com/bucket/key or bucket/key
        if (s3Url.Contains('/'))
        {
            var parts = s3Url.Split('/');
            return s3Url.StartsWith("https://") ? parts[3] : parts[0];
        }
        return s3Url;
    }

    private static string ExtractKeyFromUrl(string s3Url)
    {
        // Expected format: https://s3.region.amazonaws.com/bucket/key or bucket/key
        if (s3Url.Contains('/'))
        {
            var parts = s3Url.Split('/');
            return s3Url.StartsWith("https://") 
                ? string.Join("/", parts.Skip(4))
                : string.Join("/", parts.Skip(1));
        }
        return s3Url;
    }
}