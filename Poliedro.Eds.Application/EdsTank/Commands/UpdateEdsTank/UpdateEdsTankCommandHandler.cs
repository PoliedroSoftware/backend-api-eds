using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;
using Poliedro.Eds.Domain.EdsTank.Entities;

namespace Poliedro.Eds.Application.EdsTank.Commands.UpdateEdsTank;

    public class UpdateEdsTankCommandHandler(
        IEdsTankUpdateEdsTank EdsTankDomainEdsTank,
        IMapper mapper
    ) : IRequestHandler<UpdateEdsTankCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateEdsTankCommand request, CancellationToken cancellationToken)
        {
            var EdsTankEntity = mapper.Map<EdsTankEntity>(request);
            var result = await EdsTankDomainEdsTank.UpdateAsync(EdsTankEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }