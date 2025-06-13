public class HoseHistoryViewDto
{
    public int Id { get; set; }
    public int IdHose { get; set; }
    public int NumberHose { get; set; }
    public int IdDispenser { get; set; }
    public int DispenserNumber { get; set; }
    public int IdEds { get; set; }
    public string Eds { get; set; }
    public double AccumulatedAmount { get; set; }
    public double AccumulatedGallons { get; set; }
    public DateOnly Date { get; set; }
}
