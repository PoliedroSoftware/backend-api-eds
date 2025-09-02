using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.StrongBox.Repositories;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetList;

public class StrongBoxGetListHandler(
    IStrongBoxRepositoryGetAll _repo,
    IMapper _mapper,
    ILogger<StrongBoxGetListHandler> _logger) : IRequestHandler<StrongBoxGetList, List<StrongBoxDto>>
{
    public async Task<List<StrongBoxDto>> Handle(StrongBoxGetList request, CancellationToken cancellationToken)
    {
            var page = request.Page < 1 ? 1 : request.Page;
            var size = request.PageSize is < 1 or > 100 ? 20 : request.PageSize;

            var type = string.IsNullOrWhiteSpace(request.Type)
                ? null : request.Type.Trim().ToUpperInvariant();

            var list = await _repo.GetListAsync(
                (page - 1) * size,
                size,
                request.IdCorte,  
                type,      
                request.From,      
                request.To,        
                cancellationToken);

          
            if (list == null)
                return new List<StrongBoxDto>();

            return _mapper.Map<List<StrongBoxDto>>(list);
          
        }
}
