using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Commands.UpdatePosOfSale;

public record UpdatePosOfSaleDetailsCommand(
    int IdDetail,
    int PosId,
    string ProductCode,
    string ProductName,
    string? UnitMeasure,
    decimal? Quantity,
    decimal? UnitPrice,
    decimal? DiscountAmount,
    decimal? TaxAmount,
    decimal? TotalAmount,
    string? TaxType,
    decimal? TaxPercentage,
    string? RetentionType,
    decimal? RetentionAmount) : IRequest<Result<VoidResult, Error>>;
