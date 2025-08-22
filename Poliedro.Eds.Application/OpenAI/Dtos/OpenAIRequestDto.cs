namespace Poliedro.Eds.Application.OpenAI.Dtos;

public record OpenAIRequestDto(
    string UserMessage,
    string Model = "gpt-3.5-turbo",
    string? SystemMessage = null,
    int MaxTokens = 1000,
    double Temperature = 0.7
);