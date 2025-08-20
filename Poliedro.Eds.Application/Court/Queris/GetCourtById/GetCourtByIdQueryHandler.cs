using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Court.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.DomainService;

namespace Poliedro.Eds.Application.Court.Queris.GetCourtById;

public class GetCourtByIdQueryHandler(
        ICourtGetByIdDomainService courtGetByIdDomainService,
        IMapper mapper,
        IValidator<GetCourtByIdQuery> validator
    ) : IRequestHandler<GetCourtByIdQuery, Result<CourtDto, Error>>

{
    public async Task<Result<CourtDto, Error>> Handle(GetCourtByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Result<CourtDto, Error>.Failure(
            Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
        }

        var result = await courtGetByIdDomainService.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        var courtValueMapper = mapper.Map<CourtDto>(result.Value);
        return courtValueMapper;
    }
}
