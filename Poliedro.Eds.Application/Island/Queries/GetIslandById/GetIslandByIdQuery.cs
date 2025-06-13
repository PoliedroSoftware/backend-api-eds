using MediatR;
using Poliedro.Eds.Application.Island.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Island.Queries.GetIslandById;
    public record GetIslandByIdQuery(int Id) : IRequest<Result<IslandDto, Error>>;
    
