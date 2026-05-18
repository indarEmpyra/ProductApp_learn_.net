using System;
namespace ProductApp.Models
{
  public class Product
  {
    public int Id { get; set; } // Primary key
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; } = 0; // Default value set to 0
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Default value set to current UTC time
  }
}
