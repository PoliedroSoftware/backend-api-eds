using MediatR;
using Poliedro.Eds.Application.Compartiment.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Compartiment.Queries.GetCompartimentById;
public record GetCompartimentByIdQuery(int Id) : IRequest<Result<CompartimentDto, Error>>;
