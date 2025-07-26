public class CourtListResponseEntity
{
    public int Id { get; set; }
    public int Consecutive { get; set; }
    public int IdEds { get; set; }
    public string Eds { get; set; }
    public string Bussiness { get; set; }
    public string Islander { get; set; }
    public DateOnly DateStarttime { get; set; }
    public TimeOnly Starttime { get; set; }
    public DateOnly DateEndtime { get; set; }
    public TimeOnly Endtime { get; set; }
    public double Distinc { get; set; }
    public double TotalAccumulatedAmount { get; set; }
    public double TotalAccumulatedGallons { get; set; }

    public IEnumerable<CourtCollectionViewEntity> Collections { get; set; }
    public IEnumerable<CourtDispenserViewEntity> Dispensers { get; set; }
    public IEnumerable<CourtDocumentViewEntity> Documents { get; set; }
    public IEnumerable<CourtExpenditureViewEntity> Expenditures { get; set; }
}
