using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;
using Poliedro.Eds.Domain.EdsTank.Entities;

namespace Poliedro.Eds.Application.EdsTank.Commands.CreateEdsTank;
    public class CreateEdsTankCommandHandler(
        IEdsTankCreateEdsTank EdsTankDomainEdsTank,
        IMapper mapper) : IRequestHandler<CreateEdsTankCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateEdsTankCommand request, CancellationToken cancellationToken)
        {
            var EdsTankEntity = mapper.Map<EdsTankEntity>(request.Request);
            var result = await EdsTankDomainEdsTank.CreateAsync(EdsTankEntity);
            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }