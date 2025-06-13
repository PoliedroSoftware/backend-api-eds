using MediatR;
using Poliedro.Eds.Application.Provider.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.Provider.Queries.GellAllProvider;

public record GellAllProviderQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<ProviderDto>>;