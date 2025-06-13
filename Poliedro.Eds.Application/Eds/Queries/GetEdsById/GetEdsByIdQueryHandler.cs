using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Eds.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;

namespace Poliedro.Eds.Application.Eds.Queries.GetEdsById;

public class GetEdsByIdQueryHandler(
    IEdsGetByIdService EdsGetByIdService,
    IMapper mapper)
    : IRequestHandler<GetEdsByIdQuery, Result<EdsDto, Error>>
{
    public async Task<Result<EdsDto, Error>> Handle(GetEdsByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await EdsGetByIdService.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        return mapper.Map<EdsDto>(result.Value);
    }
}