using MediatR;
using Poliedro.Eds.Application.TypeOfCollection.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.TypeOfCollection.Queries.GellAllTypeOfCollection;

public record GellAllTypeOfCollectionQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<TypeOfCollectionDto>>;
