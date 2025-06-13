using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;

namespace Poliedro.Eds.Application.ProductCompartiment.Commands.UpdateProductCompartiment;

    public class UpdateProductCompartimentCommandHandler(
        IProductCompartimentUpdateProductCompartiment ProductCompartimentDomainProductCompartiment,
        IMapper mapper
    ) : IRequestHandler<UpdateProductCompartimentCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateProductCompartimentCommand request, CancellationToken cancellationToken)
        {
            var ProductCompartimentEntity = mapper.Map<ProductCompartimentEntity>(request);
            var result = await ProductCompartimentDomainProductCompartiment.UpdateAsync(ProductCompartimentEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }