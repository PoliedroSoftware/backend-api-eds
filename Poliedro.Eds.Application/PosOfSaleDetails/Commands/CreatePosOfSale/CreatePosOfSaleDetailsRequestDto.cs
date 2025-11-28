using System;
using System.Collections.Generic;
using System.Text;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Commands.CreatePosOfSale;

public record CreatePosOfSaleDetailsRequestDto(
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
    decimal? RetentionAmount);
