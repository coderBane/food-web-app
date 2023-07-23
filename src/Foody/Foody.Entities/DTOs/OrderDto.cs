using Foody.Entities.Models;

namespace Foody.Entities.DTOs;

#nullable disable

public class OrderDto
{
    public int Id { get; set; }

    public string OrderNo { get; set; }

    public string Lastname { get; set; }

    public string Email { get; set; }

    public decimal Total { get; set; }

    public Status Status { get; set; }

    public DateTime AddedOn { get; set; }
}

public class OrderDetailsDto : OrderDto
{
    public string DeliveryAddress { get; set; }

    public ICollection<OrderDetailDto> Items { get; set; }
}

public sealed class OrderModDto
{
    public Address Address { get; set; }

    [Required]
    public string Firstname { get; set; }

    [Required]
    public string Lastname { get; set; }

    public string Email { get; set; }

    [Required]
    public string Tel { get; set; }

    public ICollection<OrderDetailDto> OrderDetails { get; set; }
}

public sealed class OrderDetailDto
{
    public int Product { get; set; }

    [DefaultValue(1)]
    public int Quantity { get; set; }

    public override bool Equals(object obj)
    {
        return obj is OrderDetailDto dto &&
               Product == dto.Product &&
               Quantity == dto.Quantity;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Product, Quantity);
    }
}