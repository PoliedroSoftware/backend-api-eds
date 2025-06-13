using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Tank.Errors;

public class TankErrorBuilder : IError
{
    public const string TANK_CREATION_ERROR = "TankCreationErrorException";
    public const string TANK_NOT_FOUND_ERROR = "TankNotFoundErrorException";
    public static Error TankCreationException() => Error.CreateInstance(
       TANK_CREATION_ERROR,
        "Failed to create Tank due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string TANK_UPDATE_ERROR = "TankUpdateErrorException";

    public static Error TankUpdateException() => Error.CreateInstance(
       TANK_UPDATE_ERROR,
       "Failed to update Tank due to an internal error.",
       HttpStatusCode.InternalServerError);
    public static Error TankNotFoundException(int id) => Error.CreateInstance(
      TANK_NOT_FOUND_ERROR,
       $"Tank with ID {id} was not found.",
       HttpStatusCode.NotFound);

}
