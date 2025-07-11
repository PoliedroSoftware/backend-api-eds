using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Compartiment.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;

namespace Poliedro.Eds.Application.Compartiment.Queries.GetCompartimentById
{
    public class GetCompartimentByIdQueryHandler(
        ICompartimentGetByIdCompartiment compartimentDomainService,
        IMapper mapper)
        : IRequestHandler<GetCompartimentByIdQuery, Result<CompartimentDto, Error>>
    {
        public async Task<Result<CompartimentDto, Error>> Handle(GetCompartimentByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await compartimentDomainService.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<CompartimentDto>(result.Value);
        }
    }
}
