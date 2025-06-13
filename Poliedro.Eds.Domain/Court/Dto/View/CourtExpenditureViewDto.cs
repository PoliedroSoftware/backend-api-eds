   public class CourtExpenditureViewDto
   {
       public int Id { get; set; }
       public int Court { get; set; }
       public DateOnly Date { get; set; }
       public string Expenditure { get; set; }
       public double Amount { get; set; }
       public string Description { get; set; }
   }
   