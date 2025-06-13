using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;

namespace Poliedro.Eds.Application.ProductCompartiment.Commands.CreateProductCompartiment;
    public class CreateProductCompartimentCommandHandler(
        IProductCompartimentCreateProductCompartiment ProductCompartimentDomainProductCompartiment,
        IMapper mapper) : IRequestHandler<CreateProductCompartimentCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateProductCompartimentCommand request, CancellationToken cancellationToken)
        {
            var ProductCompartimentEntity = mapper.Map<ProductCompartimentEntity>(request.Request);
            var result = await ProductCompartimentDomainProductCompartiment.CreateAsync(ProductCompartimentEntity);
            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }