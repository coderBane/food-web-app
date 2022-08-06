namespace Foody.Entities.Models;

public class Product : Item
{
    [Required]
    public int CategoryId { get; set; }

    public int Quantity { get; set; }

    [Required]
    [Range(50, 50000)]
    public decimal Price { get; set; }

    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    public Category Category { get; set; } = null!;
}

