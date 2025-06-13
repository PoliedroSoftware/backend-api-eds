   public class CourtDispenserViewDto
   {
       public int Id { get; set; }
       public string Business { get; set; }
       public int IdEds { get; set; }
       public string Eds { get; set; }
       public int Dispenser { get; set; }
       public int NumberHose { get; set; }
       public double LastAccumulatedAmount { get; set; }
       public double LastAccumulatedGallons { get; set; }
       public int CodeCourt { get; set; }
       public string Islander { get; set; }
       public DateOnly DateStarttime { get; set; }
       public TimeOnly Starttime { get; set; }
       public DateOnly DateEndtime { get; set; }
       public TimeOnly Endtime { get; set; }      
       public double Distinc { get; set; }
       public string Product { get; set; }
       public double Price { get; set; }
       public string ProductType { get; set; }
       public double AccumulatedAmount { get; set; }
       public double AccumulatedGallons { get; set; }
   }
   