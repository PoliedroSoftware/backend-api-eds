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

namespace Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetById;

public class StrongBoxGetByIdHandler(IStrongBoxRepositoryGetById _repo, IMapper _mapper) : IRequestHandler<StrongBoxGetId, StrongBoxDto?>
{
    public async Task<StrongBoxDto?> Handle(StrongBoxGetId request, CancellationToken cancellationToken)
            => _mapper.Map<StrongBoxDto>(await _repo.GetByIdAsync(request.Id, cancellationToken)) ?? null;
}
