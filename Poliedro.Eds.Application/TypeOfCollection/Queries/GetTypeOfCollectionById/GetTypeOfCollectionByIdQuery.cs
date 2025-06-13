using MediatR;
using Poliedro.Eds.Application.TypeOfCollection.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.TypeOfCollection.Queries.GetTypeOfCollectionById;
    public record GetTypeOfCollectionByIdQuery (int Id) : IRequest<Result<TypeOfCollectionDto, Error>>;
