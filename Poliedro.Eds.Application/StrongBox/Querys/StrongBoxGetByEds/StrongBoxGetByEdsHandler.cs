using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.StrongBox.Repositories;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetByEds;

public class StrongBoxGetByEdsHandler(
    IStrongBoxRepositoryGetByEds repository, 
    IMapper mapper,
    ILogger<StrongBoxGetByEdsHandler> logger) 
    : IRequestHandler<StrongBoxGetByEds, List<StrongBoxDto>>
{
    public async Task<List<StrongBoxDto>> Handle(StrongBoxGetByEds request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting StrongBox records for EDS ID: {IdEds}", request.IdEds);
        
        var entities = await repository.GetByEdsAsync(request.IdEds, cancellationToken);
        var result = mapper.Map<List<StrongBoxDto>>(entities);
        
        logger.LogInformation("Found {Count} StrongBox records for EDS ID: {IdEds}", result.Count, request.IdEds);
        
        return result;
    }
}
