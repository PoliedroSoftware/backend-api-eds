using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.OpenAI.Entities;

namespace Poliedro.Eds.Domain.OpenAI.DomainOpenAI;

public interface IOpenAIChatService
{
    Task<Result<OpenAIResponseEntity, Error>> SendChatMessageAsync(OpenAIRequestEntity request);
}