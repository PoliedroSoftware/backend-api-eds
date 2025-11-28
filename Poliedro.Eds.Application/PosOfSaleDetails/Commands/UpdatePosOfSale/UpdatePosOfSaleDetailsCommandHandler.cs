using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.DomainPosOfSaleDetails;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Commands.UpdatePosOfSale
{
    public class UpdatePosOfSaleCommandHandler(
        IPosOfSaleDetailsUpdateService posOFSaleDetailsDomain,
        IMapper mapper,
        IValidator<UpdatePosOfSaleDetailsCommand> validator
    ) : IRequestHandler<UpdatePosOfSaleDetailsCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdatePosOfSaleDetailsCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var posOfSaleEntity = mapper.Map<PosOfSaleDetailsEntity>(request);
            var result = await posOFSaleDetailsDomain.UpdateAsync(posOfSaleEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }
}
