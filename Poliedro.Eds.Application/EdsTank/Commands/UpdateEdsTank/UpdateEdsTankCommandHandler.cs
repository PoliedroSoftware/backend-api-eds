using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Capacity.Commands.UpdateCapacity;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;
using Poliedro.Eds.Domain.EdsTank.Entities;

namespace Poliedro.Eds.Application.EdsTank.Commands.UpdateEdsTank;

public class UpdateEdsTankCommandHandler(
    IEdsTankUpdateEdsTank EdsTankDomainEdsTank,
    IMapper mapper,
    IValidator<UpdateEdsTankCommand> validator
    ) : IRequestHandler<UpdateEdsTankCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdateEdsTankCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var EdsTankEntity = mapper.Map<EdsTankEntity>(request);
        var result = await EdsTankDomainEdsTank.UpdateAsync(EdsTankEntity);

        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}
