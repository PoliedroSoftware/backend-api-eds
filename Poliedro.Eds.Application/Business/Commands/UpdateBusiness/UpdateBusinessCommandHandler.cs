using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Business.Commands.UpdateBusiness;

public class UpdateBusinessCommandHandler(
    IBusinessUpdateService BusinessUpdateService,
    IMapper mapper,
    IValidator<UpdateBusinessCommand> validator
    ) : IRequestHandler<UpdateBusinessCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdateBusinessCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var BusinessEntity = mapper.Map<BusinessEntity>(request);
        var result = await BusinessUpdateService.UpdateAsync(BusinessEntity);

        if (!result.IsSuccess)
            return result.Error!;
        return result.Value!;
    }
}
