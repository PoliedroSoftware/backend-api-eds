using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Application.Eds.Commands.CreateEds;

public class CreateEdsCommandHandler(
    IEdsCreateService EdsCreateService,
    IMapper mapper,
    IValidator<CreateEdsRequestDto> validator
    ) : IRequestHandler<CreateEdsCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateEdsCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var EdsEntity = mapper.Map<EdsEntity>(request.Request);
        var result = await EdsCreateService.CreateAsync(EdsEntity);
        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}
