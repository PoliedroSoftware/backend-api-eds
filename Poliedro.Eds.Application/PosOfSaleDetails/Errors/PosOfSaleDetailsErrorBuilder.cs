using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Errors;

public class PosOfSaleDetailsErrorBuilder : IError
{
    public const string POS_OF_SALE_DETAIL_CREATION_ERROR = "PosOfSaleDetailsCreationErrorException";
    public const string POS_OF_SALE_DETAIL_NOT_FOUND_ERROR = "PosOfSaleDetailsNotFoundErrorException";
    public const string POS_OF_SALE_DETAIL_LIMIT_REACHED_ERROR = "PosOfSaleDetailsLimitErrorException";
    public const string POS_OF_SALE_DETAIL_LIST_EMPTY_ERROR = "PosOfSaleDetailsListEmptyException";
    public const string POS_OF_SALE_DETAIL_UPDATE_ERROR = "PosOfSaleDetailsUpdateErrorException";

    public static Error PosOfSaleDetailsCreationException() => Error.CreateInstance(
       POS_OF_SALE_DETAIL_CREATION_ERROR,
        "Failed to create Pos of sale details due to an internal error.",
        HttpStatusCode.InternalServerError);

    public static Error PosOfSaleDetailsLimitErrorException() => Error.CreateInstance(
      POS_OF_SALE_DETAIL_CREATION_ERROR,
       "Maximum number of pos of sale details reached.",
       HttpStatusCode.BadRequest);

    public static Error PosOfSaleDetailsUpdateException() => Error.CreateInstance(
       POS_OF_SALE_DETAIL_UPDATE_ERROR,
       "Failed to update Pos of sale due detail to an internal error.",
       HttpStatusCode.InternalServerError);

    public static Error PosOfSaleDetailsNotFoundException(int id) => id == 0
        ? Error.CreateInstance(
            POS_OF_SALE_DETAIL_LIST_EMPTY_ERROR,
            "No Pos of sale details found in the system.",
            HttpStatusCode.NotFound)
        : Error.CreateInstance(
            POS_OF_SALE_DETAIL_NOT_FOUND_ERROR,
            $"Pos of sale detail with ID {id} was not found.",
            HttpStatusCode.NotFound);

}
