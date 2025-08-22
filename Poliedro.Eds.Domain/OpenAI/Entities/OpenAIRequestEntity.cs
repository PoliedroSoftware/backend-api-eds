using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Common.Entities;

namespace Poliedro.Eds.Domain.OpenAI.Entities;

public class OpenAIRequestEntity : AggregateRoot
{
    [Key]
    public int Id { get; private set; }
    public string UserMessage { get; private set; } = null!;
    public string Model { get; private set; } = null!;
    public string? SystemMessage { get; private set; }
    public int MaxTokens { get; private set; }
    public double Temperature { get; private set; }
    public string UserId { get; private set; } = null!;

    private OpenAIRequestEntity(string userMessage, string model, string? systemMessage, int maxTokens, double temperature, string userId)
    {
        Validate(userMessage, model, maxTokens, temperature, userId);
        
        UserMessage = userMessage;
        Model = model;
        SystemMessage = systemMessage;
        MaxTokens = maxTokens;
        Temperature = temperature;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }

    public static OpenAIRequestEntity Create(string userMessage, string model, string? systemMessage, int maxTokens, double temperature, string userId)
    {
        return new OpenAIRequestEntity(userMessage, model, systemMessage, maxTokens, temperature, userId);
    }

    private static void Validate(string userMessage, string model, int maxTokens, double temperature, string userId)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
            throw new ArgumentException("User message cannot be empty.", nameof(userMessage));

        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model cannot be empty.", nameof(model));

        if (maxTokens <= 0)
            throw new ArgumentException("Max tokens must be greater than zero.", nameof(maxTokens));

        if (temperature < 0 || temperature > 2)
            throw new ArgumentException("Temperature must be between 0 and 2.", nameof(temperature));

        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
    }

    // Constructor for EF Core
    protected OpenAIRequestEntity() { }
}