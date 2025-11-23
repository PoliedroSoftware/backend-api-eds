using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.PosOfSale.Errors;

public class PosOfSaleErrorBuilder : IError
{
    public const string POS_OF_SALE_CREATION_ERROR = "PosOfSaleCreationErrorException";
    public const string POS_OF_SALE_NOT_FOUND_ERROR = "PosOfSaleNotFoundErrorException";
    public const string POS_OF_SALE_LIMIT_REACHED_ERROR = "PosOfSaleLimitErrorException";
    public const string POS_OF_SALE_LIST_EMPTY_ERROR = "PosOfSaleListEmptyException";
    public const string POS_OF_SALE_UPDATE_ERROR = "PosOfSaleUpdateErrorException";

    public static Error PosOfSaleCreationException() => Error.CreateInstance(
       POS_OF_SALE_CREATION_ERROR,
        "Failed to create Pos of sale due to an internal error.",
        HttpStatusCode.InternalServerError);

    public static Error PosOfSaleLimitErrorException() => Error.CreateInstance(
      POS_OF_SALE_CREATION_ERROR,
       "Maximum number of pos of sales reached.",
       HttpStatusCode.BadRequest);

    public static Error PosOfSaleUpdateException() => Error.CreateInstance(
       POS_OF_SALE_UPDATE_ERROR,
       "Failed to update Pos of sale due to an internal error.",
       HttpStatusCode.InternalServerError);

    public static Error PosOfSaleNotFoundException(int id) => id == 0
        ? Error.CreateInstance(
            POS_OF_SALE_LIST_EMPTY_ERROR,
            "No Pos of sales found in the system.",
            HttpStatusCode.NotFound)
        : Error.CreateInstance(
            POS_OF_SALE_NOT_FOUND_ERROR,
            $"Pos of sale with ID {id} was not found.",
            HttpStatusCode.NotFound);

}
