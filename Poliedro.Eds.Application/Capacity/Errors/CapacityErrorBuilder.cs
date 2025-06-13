using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Capacity.Errors;

public class CapacityErrorBuilder : IError
{
    public const string Capacity_CREATION_ERROR = "CapacityCreationErrorException";
    public const string Capacity_NOT_FOUND_ERROR = "CapacityNotFoundErrorException";
    public static Error CapacityCreationException() => Error.CreateInstance(
       Capacity_CREATION_ERROR,
        "Failed to create Capacity due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string Capacity_UPDATE_ERROR = "CapacityUpdateErrorException";

    public static Error CapacityUpdateException() => Error.CreateInstance(
            Capacity_UPDATE_ERROR,
            "Failed to update Capacity due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error CapacityNotFoundException(int id) => Error.CreateInstance(
      Capacity_NOT_FOUND_ERROR,
       $"Capacity with ID {id} was not found.",
       HttpStatusCode.NotFound);

}