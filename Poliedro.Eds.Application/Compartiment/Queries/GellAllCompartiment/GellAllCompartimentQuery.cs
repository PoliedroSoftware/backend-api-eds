using MediatR;
using Poliedro.Eds.Application.Compartiment.Dtos;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.Compartiment.Queries.GellAllCompartiment;

public record GellAllCompartimentQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<CompartimentDto>>;
