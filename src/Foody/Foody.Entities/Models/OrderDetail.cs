namespace Foody.Entities.Models;

public sealed record OrderDetail
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Total { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}