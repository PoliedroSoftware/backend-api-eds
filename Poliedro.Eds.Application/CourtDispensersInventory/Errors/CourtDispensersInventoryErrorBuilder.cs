using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Errors;

public class CourtDispensersInventoryErrorBuilder : IError
{
    public const string COURTDISPENSERSINVENTORY_CREATION_ERROR = "CourtDispensersInventoryCreationErrorException";
    public const string COURTDISPENSERSINVENTORY_NOT_FOUND_ERROR = "CourtDispensersInventoryNotFoundErrorException";
    public static Error CourtDispensersInventoryCreationException() => Error.CreateInstance(
       COURTDISPENSERSINVENTORY_CREATION_ERROR,
        "Failed to create CourtDispensersInventory due to an internal error.",
        HttpStatusCode.InternalServerError);

    public const string COURTDISPENSERSINVENTORY_UPDATE_ERROR = "CourtDispensersInventoryUpdateErrorException";

    public static Error CourtDispensersInventoryUpdateException() => Error.CreateInstance(
       COURTDISPENSERSINVENTORY_UPDATE_ERROR,
       "Failed to update CourtDispensersInventory due to an internal error.",
       HttpStatusCode.InternalServerError);
    public static Error CourtDispensersInventoryNotFoundException(int id) => Error.CreateInstance(
      COURTDISPENSERSINVENTORY_NOT_FOUND_ERROR,
       $"CourtDispensersInventory with ID {id} was not found.",
       HttpStatusCode.NotFound);

}
