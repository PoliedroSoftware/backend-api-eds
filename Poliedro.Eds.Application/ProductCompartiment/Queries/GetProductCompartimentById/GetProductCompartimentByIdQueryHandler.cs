using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.ProductCompartiment.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;

namespace Poliedro.Eds.Application.ProductCompartiment.Queries.GetProductCompartimentById
{
    public class GetProductCompartimentByIdQueryHandler(
        IProductCompartimentGetByIdProductCompartiment ProductCompartimentDomainProductCompartiment,
        IMapper mapper)
        : IRequestHandler<GetProductCompartimentByIdQuery, Result<ProductCompartimentDto, Error>>
    {
        public async Task<Result<ProductCompartimentDto, Error>> Handle(GetProductCompartimentByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await ProductCompartimentDomainProductCompartiment.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<ProductCompartimentDto>(result.Value);
        }
    }
}
