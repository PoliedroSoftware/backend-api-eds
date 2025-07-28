using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Provider.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using System.Net;

namespace Poliedro.Eds.Application.Provider.Queries.GetProviderById;

public class GetProviderByIdQueryHandler(
    IProviderGetByIdService ProviderGetByIdService,
    IMapper mapper,
    IValidator<GetProviderByIdQuery> validator)
    : IRequestHandler<GetProviderByIdQuery, Result<ProviderDto, Error>>
{
    public async Task<Result<ProviderDto, Error>> Handle(GetProviderByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Result<ProviderDto, Error>.Failure(
            Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
        }
        var result = await ProviderGetByIdService.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        return mapper.Map<ProviderDto>(result.Value);
    }
}