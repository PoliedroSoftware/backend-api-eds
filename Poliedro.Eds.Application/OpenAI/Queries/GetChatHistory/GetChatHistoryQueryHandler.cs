using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.OpenAI.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.OpenAI.DomainOpenAI;
using System.Net;

namespace Poliedro.Eds.Application.OpenAI.Queries.GetChatHistory;

public class GetChatHistoryQueryHandler(
    IOpenAIResponseRepository responseRepository,
    IMapper mapper,
    IValidator<GetChatHistoryQuery> validator
    ) : IRequestHandler<GetChatHistoryQuery, Result<IEnumerable<OpenAIResponseDto>, Error>>
{
    public async Task<Result<IEnumerable<OpenAIResponseDto>, Error>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<IEnumerable<OpenAIResponseDto>, Error>.Failure(
                Error.CreateInstance("ValidationFailed", string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)), HttpStatusCode.BadRequest));

        var result = await responseRepository.GetByUserIdAsync(request.UserId, request.PageNumber, request.PageSize);
        
        if (!result.IsSuccess)
        {
            return Result<IEnumerable<OpenAIResponseDto>, Error>.Failure(result.Error!);
        }

        var responseDtos = mapper.Map<IEnumerable<OpenAIResponseDto>>(result.Value);
        return Result<IEnumerable<OpenAIResponseDto>, Error>.Success(responseDtos);
    }
}