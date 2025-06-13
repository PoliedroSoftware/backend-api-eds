using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.TypeOfCollection.Commands.CreateTypeOfCollection;

public record CreateTypeOfCollectionCommand (CreateTypeOfCollectionRequestDto Request) : IRequest<Result<VoidResult, Error>>;
