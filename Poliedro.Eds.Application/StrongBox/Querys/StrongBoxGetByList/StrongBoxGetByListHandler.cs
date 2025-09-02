using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.StrongBox.Repositories;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetList
{
    public class StrongBoxGetListHandler : IRequestHandler<StrongBoxGetList, List<StrongBoxDto>>
    {
        private readonly IStrongBoxRepositoryGetAll _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<StrongBoxGetListHandler> _logger;

        // Constructor para inyección de dependencias
        public StrongBoxGetListHandler(IStrongBoxRepositoryGetAll repo, IMapper mapper, ILogger<StrongBoxGetListHandler> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<StrongBoxDto>> Handle(StrongBoxGetList request, CancellationToken cancellationToken)
        {
            try
            {
                var page = request.Page < 1 ? 1 : request.Page;
                var size = request.PageSize is < 1 or > 100 ? 20 : request.PageSize;

                // Llama al repositorio solo con los parámetros de paginación, los demás pueden ser null
                var type = string.IsNullOrWhiteSpace(request.Type)
                    ? null : request.Type.Trim().ToUpperInvariant();

                var list = await _repo.GetListAsync(
                    (page - 1) * size,
                    size,
                    request.IdCorte,   // Puede ser null
                    type,      // Puede ser null
                    request.From,      // Puede ser null
                    request.To,        // Puede ser null
                    cancellationToken);

                // Validación para evitar NullReferenceException
                if (list == null)
                    return new List<StrongBoxDto>();

                //return _mapper.Map<List<StrongBoxDto>>(list);
                return list.Select(x=> new StrongBoxDto
                {
                    Id = x.Id,
                    DateTime = x.DateTime,
                    IdCorte = x.IdCorte,
                    Type = x.Type,
                    Ammount = x.Ammount,
                    Saldo = x.Saldo,
                    Note = x.Note
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en StrongBoxGetListHandler.Handle: {Message}", ex.Message);
                throw;
            }
        }
    }
}
