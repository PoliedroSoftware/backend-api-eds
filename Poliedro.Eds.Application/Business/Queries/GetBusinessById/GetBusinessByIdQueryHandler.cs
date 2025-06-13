using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Business.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Business.DomainBusiness;

namespace Poliedro.Eds.Application.Business.Queries.GetBusinessById;

public class GetBusinessByIdQueryHandler(
    IBusinessGetByIdService BusinessGetByIdService,
    IMapper mapper)
    : IRequestHandler<GetBusinessByIdQuery, Result<BusinessDto, Error>>
{
    public async Task<Result<BusinessDto, Error>> Handle(GetBusinessByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await BusinessGetByIdService.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        return mapper.Map<BusinessDto>(result.Value);
    }
}