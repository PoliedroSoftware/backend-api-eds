using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Application.Eds.Commands.UpdateEds;

public class UpdateEdsCommandHandler(
    IEdsUpdateService EdsUpdateService,
    IMapper mapper,
    IValidator<UpdateEdsCommand> validator
    ) : IRequestHandler<UpdateEdsCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdateEdsCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var EdsEntity = mapper.Map<EdsEntity>(request);
        var result = await EdsUpdateService.UpdateAsync(EdsEntity);

        if (!result.IsSuccess)
            return result.Error!;
        return result.Value!;
    }
}
