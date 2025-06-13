using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Tank.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Tank.DomainTank;

namespace Poliedro.Eds.Application.Tank.Queries.GetTankById
{
    public class GetTankByIdQueryHandler(
        ITankGetByIdTank tankDomainTank,
        IMapper mapper)
        : IRequestHandler<GetTankByIdQuery, Result<TankDto, Error>>
    {
        public async Task<Result<TankDto, Error>> Handle(GetTankByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await tankDomainTank.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<TankDto>(result.Value);
        }
    }
}
