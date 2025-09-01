using MediatR;
using Poliedro.Eds.Domain.Common.Pagination;
<<<<<<< HEAD
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
=======
>>>>>>> New-service-StrongBox
using Poliedro.Eds.Domain.Hose.Dtos;

namespace Poliedro.Eds.Application.Hose.Queries.GellAllHose;

<<<<<<< HEAD
public record GellAllHoseQuery(PaginationParams PaginationParams) : IRequest<Result<IEnumerable<HoseDto>, Error>>;
=======
public record GellAllHoseQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<HoseDto>>;
>>>>>>> New-service-StrongBox
