using Poliedro.Eds.Domain.Dispensers.Entities;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.ProductType.Entities;

namespace Poliedro.Eds.Domain.Hose.Dtos;

public record HoseDto(
    int IdHose,
    int Number,
    int IdDispensers,
    double AccumulatedGallons,
    double AccumulatedAmount,
    int IdProductType,
    DispensersEntity dispensersEntity,
    ProductTypeEntity productTypeEntity,
    EdsEntity edsEntity
);
