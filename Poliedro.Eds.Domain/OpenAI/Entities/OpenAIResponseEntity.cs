using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Common.Entities;

namespace Poliedro.Eds.Domain.OpenAI.Entities;

public class OpenAIResponseEntity : AggregateRoot
{
    [Key]
    public int Id { get; private set; }
    public int RequestId { get; private set; }
    public string ResponseContent { get; private set; } = null!;
    public string Model { get; private set; } = null!;
    public int TokensUsed { get; private set; }
    public string FinishReason { get; private set; } = null!;
    public double ProcessingTimeMs { get; private set; }

    // Navigation property
    public OpenAIRequestEntity Request { get; private set; } = null!;

    private OpenAIResponseEntity(int requestId, string responseContent, string model, int tokensUsed, string finishReason, double processingTimeMs)
    {
        Validate(responseContent, model, tokensUsed, finishReason);
        
        RequestId = requestId;
        ResponseContent = responseContent;
        Model = model;
        TokensUsed = tokensUsed;
        FinishReason = finishReason;
        ProcessingTimeMs = processingTimeMs;
        CreatedAt = DateTime.UtcNow;
    }

    public static OpenAIResponseEntity Create(int requestId, string responseContent, string model, int tokensUsed, string finishReason, double processingTimeMs)
    {
        return new OpenAIResponseEntity(requestId, responseContent, model, tokensUsed, finishReason, processingTimeMs);
    }

    private static void Validate(string responseContent, string model, int tokensUsed, string finishReason)
    {
        if (string.IsNullOrWhiteSpace(responseContent))
            throw new ArgumentException("Response content cannot be empty.", nameof(responseContent));

        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model cannot be empty.", nameof(model));

        if (tokensUsed < 0)
            throw new ArgumentException("Tokens used cannot be negative.", nameof(tokensUsed));

        if (string.IsNullOrWhiteSpace(finishReason))
            throw new ArgumentException("Finish reason cannot be empty.", nameof(finishReason));
    }

    // Constructor for EF Core
    protected OpenAIResponseEntity() { }
}