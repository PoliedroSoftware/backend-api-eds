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
                var list = await _repo.GetListAsync(
                    (page - 1) * size,
                    size,
                    request.IdCorte,   // Puede ser null
                    request.Type,      // Puede ser null
                    request.From,      // Puede ser null
                    request.To,        // Puede ser null
                    cancellationToken);

                // Validación para evitar NullReferenceException
                if (list == null)
                    return new List<StrongBoxDto>();

                return _mapper.Map<List<StrongBoxDto>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en StrongBoxGetListHandler.Handle: {Message}", ex.Message);
                throw;
            }
        }
    }
}
