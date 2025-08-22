using MediatR;
using Poliedro.Eds.Application.OpenAI.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.OpenAI.Commands.SendMessage;

public record SendOpenAIMessageCommand(OpenAIRequestDto Request, string UserId) : IRequest<Result<OpenAIResponseDto, Error>>;