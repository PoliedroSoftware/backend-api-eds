using MediatR;
using Poliedro.Eds.Application.OpenAI.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.OpenAI.Queries.GetChatHistory;

public record GetChatHistoryQuery(string UserId, int PageNumber = 1, int PageSize = 10) : IRequest<Result<IEnumerable<OpenAIResponseDto>, Error>>;