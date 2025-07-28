using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;
using System.Net;

namespace Poliedro.Eds.Application.ProductCompartiment.Commands.CreateProductCompartiment;

public class CreateProductCompartimentCommandHandler(
    IProductCompartimentCreateProductCompartiment ProductCompartimentDomainProductCompartiment,
    IMapper mapper,
    IValidator<CreateProductCompartimentRequestDto> validator
    ) : IRequestHandler<CreateProductCompartimentCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateProductCompartimentCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var ProductCompartimentEntity = mapper.Map<ProductCompartimentEntity>(request.Request);
        var result = await ProductCompartimentDomainProductCompartiment.CreateAsync(ProductCompartimentEntity);
        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}