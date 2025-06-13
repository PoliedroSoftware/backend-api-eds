using MediatR;
using Poliedro.Eds.Application.EdsTank.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.EdsTank.Queries.GetEdsTankById;
    public record GetEdsTankByIdQuery (int Id) : IRequest<Result<EdsTankDto, Error>>;
