using MediatR;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Queries.GetAllPosOfSale;

public record GellAllPosOfSaleDetailsQuery(PaginationParams PaginationParams) : IRequest<Result<IEnumerable<PosOfSaleDetailsEntity>, Error>>;
