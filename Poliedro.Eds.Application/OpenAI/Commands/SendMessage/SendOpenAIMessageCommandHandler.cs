using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.OpenAI.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.OpenAI.DomainOpenAI;
using Poliedro.Eds.Domain.OpenAI.Entities;

namespace Poliedro.Eds.Application.OpenAI.Commands.SendMessage;

public class SendOpenAIMessageCommandHandler(
    IOpenAIChatService openAiChatService,
    IMapper mapper,
    IValidator<OpenAIRequestDto> validator
    ) : IRequestHandler<SendOpenAIMessageCommand, Result<OpenAIResponseDto, Error>>
{
    public async Task<Result<OpenAIResponseDto, Error>> Handle(SendOpenAIMessageCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<OpenAIResponseDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)), HttpStatusCode.BadRequest));

        var openAiRequest = OpenAIRequestEntity.Create(
            request.Request.UserMessage,
            request.Request.Model,
            request.Request.SystemMessage,
            request.Request.MaxTokens,
            request.Request.Temperature,
            request.UserId
        );

        var result = await openAiChatService.SendChatMessageAsync(openAiRequest);
        
        if (!result.IsSuccess)
        {
            return Result<OpenAIResponseDto, Error>.Failure(result.Error!);
        }

        var responseDto = mapper.Map<OpenAIResponseDto>(result.Value);
        return Result<OpenAIResponseDto, Error>.Success(responseDto);
    }
}