public class HoseViewEntity
{
    public int IdHose { get; set; }
    public int HoseNumber { get; set; }
    public int Dispenser { get; set; }
    public double AccumulatedGallons { get; set; }
    public double AccumulatedAmount { get; set; }
    public int ProductType { get; set; }
    public double Price { get; set; }

    // Dispenser details
    public int IdDispenser { get; set; }
    public string Code { get; set; } = string.Empty;
    public int Number { get; set; }
    public int IdDispenserType { get; set; }
    public int IdEds { get; set; }
    public int IdIsland { get; set; }
    public int NumberHose { get; set; }

    // Product Type details
    public int IdProductType { get; set; }
    public string Description { get; set; } = string.Empty;

    // Eds details
    public int EdsId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Nit { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Sicom { get; set; } = string.Empty;
    public int IdBusiness { get; set; }
}
