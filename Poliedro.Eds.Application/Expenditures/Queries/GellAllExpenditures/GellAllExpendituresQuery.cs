using MediatR;
using Poliedro.Eds.Application.Expenditures.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.Expenditures.Queries.GellAllExpenditures;

public record GellAllExpendituresQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<ExpendituresDto>>;
