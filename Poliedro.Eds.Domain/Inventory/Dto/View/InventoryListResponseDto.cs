namespace Poliedro.Eds.Domain.Inventory.Dto.View;

public class InventoryListResponseDto
{
    public IEnumerable<BusinessDto> Businesses { get; set; }
}

public class BusinessDto
{
    public int IdBusiness { get; set; }
    public string Business { get; set; }
    public IEnumerable<EdsDto> Eds { get; set; }
}

public class EdsDto
{
    public int IdEds { get; set; }
    public string Eds { get; set; }
    public IEnumerable<TankDto> Tanks { get; set; }
}

public class TankDto
{
    public int IdTank { get; set; }
    public string Tank { get; set; }
    public double TankCapacity { get; set; }
    public IEnumerable<CompartmentDto> Compartments { get; set; }
}

public class CompartmentDto
{
    public int IdCompartment { get; set; }
    public int Compartment { get; set; }
    public int IdProduct { get; set; }
    public string Product { get; set; }
    public double Stock { get; set; }
}
