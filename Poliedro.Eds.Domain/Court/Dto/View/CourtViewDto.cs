public class CourtViewDto
   {
    public int Id { get; set; }
    public DateOnly DateStarttime { get; set; }
    public DateOnly DateEndtime { get; set; }
    public int Consecutive { get; set; }
    public int IdEds { get; set; }
    public string Eds { get; set; }
    public string Bussiness { get; set; }
    public string Islander { get; set; }
    public TimeOnly Starttime { get; set; }
    public TimeOnly Endtime { get; set; }
    public double Distinc { get; set; }
    public double TotalAccumulatedAmount { get; set; }
    public double TotalAccumulatedGallons { get; set; }
}
   