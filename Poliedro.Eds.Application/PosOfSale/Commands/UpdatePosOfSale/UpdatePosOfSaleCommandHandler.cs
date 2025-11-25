using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Hose.Commands.UpdateHose;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Domain.PointOfSale.Entities;
using Poliedro.Eds.Domain.PosOfSale.DomainPosOfSale;

namespace Poliedro.Eds.Application.PosOfSale.Commands.UpdatePosOfSale
{
    public class UpdatePosOfSaleCommandHandler(
        IPosOfSaleUpdateService PosOFSaleDomain,
        IMapper mapper,
        IValidator<UpdatePosOfSaleCommand> validator
    ) : IRequestHandler<UpdatePosOfSaleCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdatePosOfSaleCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var posOfSaleEntity = mapper.Map<PosOfSaleEntity>(request);
            var result = await PosOFSaleDomain.UpdateAsync(posOfSaleEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }
}
