using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Dispensers.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Dispensers.DomainDispensers;

namespace Poliedro.Eds.Application.Dispensers.Queries.GetDispensersById
{
    public class GetDispensersByIdQueryHandler(
        IDispensersGetByIdDispensers dispensersDomainService,
        IMapper mapper)
        : IRequestHandler<GetDispensersByIdQuery, Result<DispensersDto, Error>>
    {
        public async Task<Result<DispensersDto, Error>> Handle(GetDispensersByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await dispensersDomainService.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<DispensersDto>(result.Value);
        }
    }
}
