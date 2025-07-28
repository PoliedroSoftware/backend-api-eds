namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos;

public class MasterDto
{
    public IEnumerable<CapacityDto> Capacities { get; set; }
    public IEnumerable<CompartimentDto> Compartiments { get; set; }
    public IEnumerable<EdsDto> Eds { get; set; }
    public IEnumerable<InventoryDto> Inventory { get; set; }
    public IEnumerable<ProductDto> Products { get; set; }
    public IEnumerable<ProviderDto> Providers { get; set; }
    public IEnumerable<ShoppingDto> Shoppings { get; set; }
    public IEnumerable<TypeOfCollectionDto> TypeOfCollections { get; set; }
    public IEnumerable<Court.CourtListResponseDto> Court { get; set; }
}
