using System.ComponentModel.DataAnnotations;
using MediatR;
using Poliedro.Eds.Application.TypeOfCollection.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.TypeOfCollection.Commands.UpdateTypeOfCollection;

public record UpdateTypeOfCollectionCommand(int IdTypeOfCollection, string Description) : IRequest<Result<VoidResult, Error>>;
