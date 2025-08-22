using System.Diagnostics;
using System.Net;
using OpenAI;
using OpenAI.Chat;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.OpenAI.DomainOpenAI;
using Poliedro.Eds.Domain.OpenAI.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.OpenAI.DomainOpenAI.Impl;

public class OpenAIChatService(
    OpenAIClient openAiClient,
    IOpenAIRequestRepository requestRepository,
    IOpenAIResponseRepository responseRepository
    ) : IOpenAIChatService
{
    public async Task<Result<OpenAIResponseEntity, Error>> SendChatMessageAsync(OpenAIRequestEntity request)
    {
        try
        {
            // Save request to database first
            var requestResult = await requestRepository.CreateAsync(request);
            if (!requestResult.IsSuccess)
            {
                return Result<OpenAIResponseEntity, Error>.Failure(requestResult.Error!);
            }

            var stopwatch = Stopwatch.StartNew();
            
            // Build the chat messages
            var messages = new List<ChatMessage>();
            
            if (!string.IsNullOrWhiteSpace(request.SystemMessage))
            {
                messages.Add(new SystemChatMessage(request.SystemMessage));
            }
            
            messages.Add(new UserChatMessage(request.UserMessage));

            // Create chat completion options
            var chatOptions = new ChatCompletionOptions
            {
                Temperature = (float)request.Temperature
            };
            
            // Set max tokens via the options
            if (request.MaxTokens > 0)
            {
                chatOptions.MaxOutputTokenCount = request.MaxTokens;
            }

            // Send request to OpenAI
            var chatClient = openAiClient.GetChatClient(request.Model);
            var completion = await chatClient.CompleteChatAsync(messages, chatOptions);
            
            stopwatch.Stop();

            // Create response entity
            var responseContent = completion.Value.Content.FirstOrDefault()?.Text ?? string.Empty;
            var tokensUsed = completion.Value.Usage?.TotalTokenCount ?? 0;
            var finishReason = completion.Value.FinishReason.ToString();

            var responseEntity = OpenAIResponseEntity.Create(
                request.Id,
                responseContent,
                request.Model,
                tokensUsed,
                finishReason,
                stopwatch.Elapsed.TotalMilliseconds
            );

            // Save response to database
            var responseResult = await responseRepository.CreateAsync(responseEntity);
            if (!responseResult.IsSuccess)
            {
                return Result<OpenAIResponseEntity, Error>.Failure(responseResult.Error!);
            }

            return Result<OpenAIResponseEntity, Error>.Success(responseEntity);
        }
        catch (Exception ex)
        {
            return Result<OpenAIResponseEntity, Error>.Failure(
                Error.CreateInstance("OpenAIError", ex.Message, HttpStatusCode.InternalServerError));
        }
    }
}