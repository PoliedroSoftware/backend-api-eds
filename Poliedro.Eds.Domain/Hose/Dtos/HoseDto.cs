using Poliedro.Eds.Domain.Dispensers.Entities;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.ProductType.Entities;

namespace Poliedro.Eds.Domain.Hose.Dtos;

public record HoseDto
{
    public HoseDto(object idHose, object number, object idDispenser, object accumulatedGallons, object accumulatedAmount, object idProductType, object price, DispensersEntity dispensersEntity, ProductTypeEntity productTypeEntity, EdsEntity edsEntity)
    {
        IdHose1 = idHose;
        Number1 = number;
        IdDispenser = idDispenser;
        AccumulatedGallons1 = accumulatedGallons;
        AccumulatedAmount1 = accumulatedAmount;
        IdProductType1 = idProductType;
        Price1 = price;
        DispensersEntity1 = dispensersEntity;
        ProductTypeEntity1 = productTypeEntity;
        EdsEntity1 = edsEntity;
    }

    public int IdHose { get; init; }
    public int Number { get; init; }
    public int IdDispensers { get; init; }
    public double AccumulatedGallons { get; init; }
    public double AccumulatedAmount { get; init; }
    public int IdProductType { get; init; }
    public double Price { get; init; }
    public DispensersEntity DispensersEntity { get; init; }
    public ProductTypeEntity ProductTypeEntity { get; init; }
    public EdsEntity EdsEntity { get; init; }
    public object IdHose1 { get; }
    public object Number1 { get; }
    public object IdDispenser { get; }
    public object AccumulatedGallons1 { get; }
    public object AccumulatedAmount1 { get; }
    public object IdProductType1 { get; }
    public object Price1 { get; }
    public DispensersEntity DispensersEntity1 { get; }
    public ProductTypeEntity ProductTypeEntity1 { get; }
    public EdsEntity EdsEntity1 { get; }
}
