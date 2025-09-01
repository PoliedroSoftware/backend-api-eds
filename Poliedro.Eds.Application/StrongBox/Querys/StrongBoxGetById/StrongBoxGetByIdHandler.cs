using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Org.BouncyCastle.Security;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.StrongBox.Repositories;

namespace Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetById
{
    public class StrongBoxGetByIdHandler : IRequestHandler<StrongBoxGetId, StrongBoxDto?>
    {
        private readonly IStrongBoxRepositoryGetById _repo;
        private readonly IMapper _mapper;

        public StrongBoxGetByIdHandler(IStrongBoxRepositoryGetById repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<StrongBoxDto?> Handle(StrongBoxGetId request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.Id, cancellationToken);
            return entity is null ? null : _mapper.Map<StrongBoxDto>(entity);
        }
    }
}
