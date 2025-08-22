using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.OpenAI.Entities;

namespace Poliedro.Eds.Domain.OpenAI.DomainOpenAI;

public interface IOpenAIRequestRepository
{
    Task<Result<OpenAIRequestEntity, Error>> CreateAsync(OpenAIRequestEntity request);
    Task<Result<IEnumerable<OpenAIRequestEntity>, Error>> GetByUserIdAsync(string userId, int pageNumber, int pageSize);
}

public interface IOpenAIResponseRepository
{
    Task<Result<OpenAIResponseEntity, Error>> CreateAsync(OpenAIResponseEntity response);
    Task<Result<IEnumerable<OpenAIResponseEntity>, Error>> GetByUserIdAsync(string userId, int pageNumber, int pageSize);
}