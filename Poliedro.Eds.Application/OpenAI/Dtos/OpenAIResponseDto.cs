namespace Poliedro.Eds.Application.OpenAI.Dtos;

public record OpenAIResponseDto(
    int Id,
    string ResponseContent,
    string Model,
    int TokensUsed,
    string FinishReason,
    double ProcessingTimeMs,
    DateTime CreatedAt
);