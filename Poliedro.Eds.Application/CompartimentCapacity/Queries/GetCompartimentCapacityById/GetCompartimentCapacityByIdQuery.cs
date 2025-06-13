using MediatR;
using Poliedro.Eds.Application.CompartimentCapacity.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.CompartimentCapacity.Queries.GetCompartimentCapacityById;
    public record GetCompartimentCapacityByIdQuery (int Id) : IRequest<Result<CompartimentCapacityDto, Error>>;
