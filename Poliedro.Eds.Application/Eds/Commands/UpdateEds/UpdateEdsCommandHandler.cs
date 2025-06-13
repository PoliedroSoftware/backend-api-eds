using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Eds.Commands.UpdateEds;

public class UpdateEdsCommandHandler(IEdsUpdateService EdsUpdateService, IMapper mapper) : IRequestHandler<UpdateEdsCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdateEdsCommand request,
        CancellationToken cancellationToken)
    {
        var EdsEntity = mapper.Map<EdsEntity>(request);
        var result = await EdsUpdateService.UpdateAsync(EdsEntity);

        if (!result.IsSuccess)
            return result.Error!;
        return result.Value!;
    }
}