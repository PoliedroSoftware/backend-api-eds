using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Capacity.Commands.UpdateCapacity;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;

namespace Poliedro.Eds.Application.ProductCompartiment.Commands.UpdateProductCompartiment;

public class UpdateProductCompartimentCommandHandler(
    IProductCompartimentUpdateProductCompartiment ProductCompartimentDomainProductCompartiment,
    IMapper mapper,
    IValidator<UpdateProductCompartimentCommand> validator
    ) : IRequestHandler<UpdateProductCompartimentCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdateProductCompartimentCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var ProductCompartimentEntity = mapper.Map<ProductCompartimentEntity>(request);
        var result = await ProductCompartimentDomainProductCompartiment.UpdateAsync(ProductCompartimentEntity);

        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}
