using MediatR;
using Poliedro.Eds.Application.Expenditures.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Expenditures.Queries.GetExpendituresById;
    public record GetExpendituresByIdQuery (int Id) : IRequest<Result<ExpendituresDto, Error>>;
