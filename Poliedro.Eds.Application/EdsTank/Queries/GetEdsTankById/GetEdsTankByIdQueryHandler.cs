using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.EdsTank.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;

namespace Poliedro.Eds.Application.EdsTank.Queries.GetEdsTankById;
    public class GetEdsTankByIdQueryHandler(
        IEdsTankGetByIdEdsTank EdsTankDomainEdsTank,
        IMapper mapper)
        : IRequestHandler<GetEdsTankByIdQuery, Result<EdsTankDto, Error>>
    {
        public async Task<Result<EdsTankDto, Error>> Handle(GetEdsTankByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await EdsTankDomainEdsTank.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<EdsTankDto>(result.Value);
        }
    }