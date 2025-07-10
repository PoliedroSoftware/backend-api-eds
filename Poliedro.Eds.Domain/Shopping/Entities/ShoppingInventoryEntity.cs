using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.Shopping.Entities;

public class ShoppingInventoryEntity
{
    public int IdInventory { get; set; }
    public DateTime Date { get; set; }
    public string ReferenceType { get; set; }
    public int ReferenceId { get; set; }
}