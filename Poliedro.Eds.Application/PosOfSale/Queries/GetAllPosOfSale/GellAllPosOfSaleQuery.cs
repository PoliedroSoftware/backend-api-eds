using MediatR;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSale.Dtos;

namespace Poliedro.Eds.Application.PosOfSale.Queries.GetAllPosOfSale;

public record GellAllPosOfSaleQuery(PaginationParams PaginationParams) : IRequest<Result<IEnumerable<PosOfSaleDto>, Error>>;
