using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Island.Errors;

public class IslandErrorBuilder : IError
{
    public const string ISLAND_CREATION_ERROR = "IslandCreationErrorException";
    public const string ISLAND_NOT_FOUND_ERROR = "IslandNotFoundErrorException";
    public static Error IslandCreationException() => Error.CreateInstance(
       ISLAND_CREATION_ERROR,
        "Failed to create Island due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string ISLAND_UPDATE_ERROR = "IslandUpdateErrorException";

    public static Error IslandUpdateException() => Error.CreateInstance(
            ISLAND_UPDATE_ERROR,
            "Failed to update Island due to an internal error.",
            HttpStatusCode.InternalServerError);
    public static Error IslandNotFoundException(int id) => Error.CreateInstance(
      ISLAND_NOT_FOUND_ERROR,
       $"Island with ID {id} was not found.",
       HttpStatusCode.NotFound);

}
