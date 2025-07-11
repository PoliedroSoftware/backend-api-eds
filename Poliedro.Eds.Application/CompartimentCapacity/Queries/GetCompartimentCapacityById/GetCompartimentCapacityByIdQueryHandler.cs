using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.CompartimentCapacity.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;

namespace Poliedro.Eds.Application.CompartimentCapacity.Queries.GetCompartimentCapacityById;

    public class GetCompartimentCapacityByIdQueryHandler(
        ICompartimentCapacityGetByIdCompartimentCapacity CompartimentCapacityDomainCompartimentCapacity,
        IMapper mapper)
        : IRequestHandler<GetCompartimentCapacityByIdQuery, Result<CompartimentCapacityDto, Error>>
    {
        public async Task<Result<CompartimentCapacityDto, Error>> Handle(GetCompartimentCapacityByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await CompartimentCapacityDomainCompartimentCapacity.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<CompartimentCapacityDto>(result.Value);
        }
    }