using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;
using Poliedro.Eds.Domain.EdsTank.Entities;

namespace Poliedro.Eds.Application.EdsTank.Commands.CreateEdsTank;

public class CreateEdsTankCommandHandler(
    IEdsTankCreateEdsTank EdsTankDomainEdsTank,
    IMapper mapper,
    IValidator<CreateEdsTankRequestDto> validator
    ) : IRequestHandler<CreateEdsTankCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateEdsTankCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var EdsTankEntity = mapper.Map<EdsTankEntity>(request.Request);
        var result = await EdsTankDomainEdsTank.CreateAsync(EdsTankEntity);
        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}
