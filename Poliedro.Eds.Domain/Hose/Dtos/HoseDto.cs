using Poliedro.Eds.Domain.Dispensers.Entities;
using Poliedro.Eds.Domain.Eds.Entities;
<<<<<<< HEAD
using Poliedro.Eds.Domain.Product.Entities;
=======
>>>>>>> New-service-StrongBox
using Poliedro.Eds.Domain.ProductType.Entities;

namespace Poliedro.Eds.Domain.Hose.Dtos;

<<<<<<< HEAD
// Clean DTOs without audit fields
public record CleanDispensersDto(
    int Id,
    string Code,
    int Number,
    int DispenserTypeId,
    int EdsId,
    int IdIsland,
    int HoseNumber    
);

public record CleanEdsDto(
    int IdEds,
    string Name,
    string Nit,
    string Address,
    string Sicom,
    int IdBusiness
);

public record CleanProductTypeDto(
    int IdProductType,
    string Description
);

public record CleanProductDto(
    int IdProduct,
    string Name,
    int IdProductType,
    double PurchasePrice,
    double SellPrice,
    double Stock,
    DateTime date
);

public record HoseDto(
    int IdHose,
    int IdDispensers,
    int Number,
    double AccumulatedAmount,
    double AccumulatedGallons,
    int IdProductType,
    int IdCompartiment,
    CleanDispensersDto? DispensersEntity,
    CleanProductTypeDto? ProductTypeEntity,
    CleanEdsDto? EdsEntity,
    CleanProductDto? ProductEntity
=======
public record HoseDto(
    int IdHose,
    int Number,
    int IdDispensers,
    double AccumulatedGallons,
    double AccumulatedAmount,
    int IdProductType,
    double Price,
    DispensersEntity dispensersEntity,
    ProductTypeEntity productTypeEntity,
    EdsEntity edsEntity
>>>>>>> New-service-StrongBox
);
