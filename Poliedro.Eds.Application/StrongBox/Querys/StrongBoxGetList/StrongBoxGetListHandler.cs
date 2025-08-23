using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.StrongBox.Repositories;

namespace Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetList
{
    public class StrongBoxGetListHandler : IRequestHandler<StrongBoxGetList, List<StrongBoxDto>>
    {
        private readonly IStrongBoxRepository _repo;
        private readonly IMapper _mapper;

        public async Task<List<StrongBoxDto>> Handle(StrongBoxGetList request, CancellationToken cancellationToken)
        {
            var page = request.Page < 1 ? 1 : request.Page;
            var size = request.PageSize  is < 1 or > 100 ? 20 : request.PageSize;

            var list = await _repo.GetListAsync(
                (page - 1) * size,
                size,
                request.IdCorte,
                request.Type,
                request.From,
                request.To,
                cancellationToken);
            return list.ConvertAll(x => _mapper.Map<StrongBoxDto>(x));
        }
    }
}
