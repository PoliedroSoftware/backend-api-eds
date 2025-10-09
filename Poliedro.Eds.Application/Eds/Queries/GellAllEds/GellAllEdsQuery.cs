using MediatR;
using Poliedro.Eds.Application.Eds.Dtos;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.Eds.Queries.GellAllEds;

public record GellAllEdsQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<EdsDto>>;
