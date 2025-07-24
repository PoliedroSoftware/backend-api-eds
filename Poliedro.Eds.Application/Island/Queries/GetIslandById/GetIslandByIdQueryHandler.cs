using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Island.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Island.DomainIsland;
using System.Net;

namespace Poliedro.Eds.Application.Island.Queries.GetIslandById
{
    public class GetIslandByIdQueryHandler(
        IIslandGetByIdIsland islandDomainIsland,
        IMapper mapper,
        IValidator<GetIslandByIdQuery> validator)
        : IRequestHandler<GetIslandByIdQuery, Result<IslandDto, Error>>
    {
        public async Task<Result<IslandDto, Error>> Handle(GetIslandByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<IslandDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await islandDomainIsland.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<IslandDto>(result.Value);
        }
    }
}
