using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Provider.Entities;
using System.Net;

namespace Poliedro.Eds.Application.Provider.Commands.UpdateProvider;

public class UpdateProviderCommandHandler(
    IProviderUpdateService ProviderUpdateService,
    IMapper mapper,
    IValidator<UpdateProviderCommand> validator
    ) : IRequestHandler<UpdateProviderCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdateProviderCommand request,CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var ProviderEntity = mapper.Map<ProviderEntity>(request);
        var result = await ProviderUpdateService.UpdateAsync(ProviderEntity);

        if (!result.IsSuccess)
            return result.Error!;
        return result.Value!;
    }
}