using MediatR;
using Poliedro.Eds.Application.Business.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.Business.Queries.GellAllBusiness;

public record GellAllBusinessQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<BusinessDto>>;