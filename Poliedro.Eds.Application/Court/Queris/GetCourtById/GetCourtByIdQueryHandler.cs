using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Court.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.DomainService;

namespace Poliedro.Eds.Application.Court.Queris.GetCourtById;

public class GetCourtByIdQueryHandler(
        ICourtGetByIdDomainService courtGetByIdDomainService,
        IMapper mapper) : IRequestHandler<GetCourtByIdQuery, Result<CourtDto, Error>>

{
    public async Task<Result<CourtDto, Error>> Handle(GetCourtByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await courtGetByIdDomainService.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        var courtValueMapper = mapper.Map<CourtDto>(result.Value);
        return courtValueMapper;
    }
}
