using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.Business.Errors;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Business.Exepction;
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
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", errorMessages, HttpStatusCode.BadRequest));
        }

        var existingBusiness = await BusinessUpdateService.GetByIdAsync(request.IdBusiness); 

        if (existingBusiness == null)
        {
            return Result<VoidResult, Error>.Failure(
                BusinessErrorBuilder.BusinessNotFoundException(request.IdBusiness));
        }

        try
        {

            existingBusiness.Update(request.Name, request.Context); 

            var result = await BusinessUpdateService.UpdateAsync(existingBusiness);
            return result;
        }
        catch (BusinessDomainException ex)
        {
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("BusinessValidationError", ex.Message, HttpStatusCode.BadRequest));
        }
        catch (Exception ex)
        {
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("UnexpectedError", ex.Message, HttpStatusCode.InternalServerError));
        }
    }

}
