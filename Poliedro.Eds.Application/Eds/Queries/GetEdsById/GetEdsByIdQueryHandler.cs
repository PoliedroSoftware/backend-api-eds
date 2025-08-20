using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Eds.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;

namespace Poliedro.Eds.Application.Eds.Queries.GetEdsById;

public class GetEdsByIdQueryHandler(
    IEdsGetByIdService EdsGetByIdService,
    IMapper mapper,
    IValidator<GetEdsByIdQuery> validator)
    : IRequestHandler<GetEdsByIdQuery, Result<EdsDto, Error>>
{
    public async Task<Result<EdsDto, Error>> Handle(GetEdsByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Result<EdsDto, Error>.Failure(
            Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
        }

        var result = await EdsGetByIdService.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        return mapper.Map<EdsDto>(result.Value);
    }
}
