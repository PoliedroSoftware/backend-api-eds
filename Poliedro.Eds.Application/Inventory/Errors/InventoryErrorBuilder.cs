using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Inventory.Errors;

public class InventoryErrorBuilder : IError
{
    public const string INVENTORY_QUERY_ERROR = "InventoryQueryErrorException";

    public static Error InventoryQueryException() => Error.CreateInstance(
        INVENTORY_QUERY_ERROR,
        "Failed to retrieve inventory data due to an internal error.",
        HttpStatusCode.InternalServerError);
}

