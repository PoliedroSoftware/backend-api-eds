using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Provider.Entities;
using System.Net;

namespace Poliedro.Eds.Application.Provider.Commands.CreateProvider;

public class CreateProviderCommandHandler(
    IProviderCreateService ProviderCreateService,
    IMapper mapper,
    IValidator<CreateProviderRequestDto> validator
    ) : IRequestHandler<CreateProviderCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var ProviderEntity = mapper.Map<ProviderEntity>(request.Request);
        var result = await ProviderCreateService.CreateAsync(ProviderEntity);
        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}