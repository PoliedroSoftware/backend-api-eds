using MediatR;
using Poliedro.Eds.Application.TypeOfCollection.Dtos;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Common.Results;
using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Application.TypeOfCollection.Commands.UpdateTypeOfCollection;
public record UpdateTypeOfCollectionCommand(int IdTypeOfCollection,string Description) : IRequest<Result<VoidResult, Error>>;