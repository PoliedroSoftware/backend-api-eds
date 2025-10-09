using MediatR;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.Islander.Queries.GellAllIslander;

public record GellAllIslanderQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<IslanderDto>>;
