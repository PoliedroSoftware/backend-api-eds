using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Application.Eds.Commands.CreateEds;

public class CreateEdsCommandHandler(
    IEdsCreateService EdsCreateService,
    IMapper mapper) : IRequestHandler<CreateEdsCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateEdsCommand request, CancellationToken cancellationToken)
    {
            var EdsEntity = mapper.Map<EdsEntity>(request.Request);
        var result = await EdsCreateService.CreateAsync(EdsEntity);
        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}