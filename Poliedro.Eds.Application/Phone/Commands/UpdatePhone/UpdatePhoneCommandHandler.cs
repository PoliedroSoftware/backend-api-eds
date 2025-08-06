using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Phone.DomainServices.Update;
using Poliedro.Eds.Domain.Phone.Entities;

namespace Poliedro.Eds.Application.Phone.Commands.UpdatePhone;

public class UpdatePhoneCommandHandler(
    IPhoneUpdateService phoneUpdateService,
    IMapper mapper,
    IValidator<UpdatePhoneCommand> validator
    ) : IRequestHandler<UpdatePhoneCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdatePhoneCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var phoneEntity = mapper.Map<PhoneEntity>(request);
        var result = await phoneUpdateService.UpdateAsync(phoneEntity);

        if (!result.IsSuccess)
            return result.Error!;
        return result.Value!;
    }
}
