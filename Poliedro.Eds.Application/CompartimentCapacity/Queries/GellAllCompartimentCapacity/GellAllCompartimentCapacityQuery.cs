using MediatR;
using Poliedro.Eds.Application.CompartimentCapacity.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.CompartimentCapacity.Queries.GellAllCompartimentCapacity;

public record GellAllCompartimentCapacityQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<CompartimentCapacityDto>>;
