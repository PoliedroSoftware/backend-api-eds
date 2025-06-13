using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Capacity.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;

namespace Poliedro.Eds.Application.Capacity.Queries.GetCapacityById;
public class GetCapacityByIdQueryHandler(
    ICapacityGetByIdService CapacityGetByIdService,
    IMapper mapper)
    : IRequestHandler<GetCapacityByIdQuery, Result<CapacityDto, Error>>
{
    public async Task<Result<CapacityDto, Error>> Handle(GetCapacityByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await CapacityGetByIdService.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        return mapper.Map<CapacityDto>(result.Value);
    }
}