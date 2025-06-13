namespace Poliedro.Eds.Domain.Shopping.Entities;

public class ShoppingEntity
{
    public int IdShopping { get; set; } 
    public string Invoice { get; set; } = default!; 
    public DateTime Date { get; set; } 
    public int IdProvider { get; set; } 
    public int IdCategory { get; set; } 
    public double Amount { get; set; } 
}


