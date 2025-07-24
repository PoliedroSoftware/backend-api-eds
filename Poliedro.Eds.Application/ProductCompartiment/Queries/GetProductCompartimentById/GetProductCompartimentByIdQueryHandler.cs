using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.ProductCompartiment.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
using System.Net;

namespace Poliedro.Eds.Application.ProductCompartiment.Queries.GetProductCompartimentById
{
    public class GetProductCompartimentByIdQueryHandler(
        IProductCompartimentGetByIdProductCompartiment ProductCompartimentDomainProductCompartiment,
        IMapper mapper,
        IValidator<GetProductCompartimentByIdQuery> validator)
        : IRequestHandler<GetProductCompartimentByIdQuery, Result<ProductCompartimentDto, Error>>
    {
        public async Task<Result<ProductCompartimentDto, Error>> Handle(GetProductCompartimentByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<ProductCompartimentDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await ProductCompartimentDomainProductCompartiment.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<ProductCompartimentDto>(result.Value);
        }
    }
}
