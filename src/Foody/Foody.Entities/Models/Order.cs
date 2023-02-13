namespace Foody.Entities.Models;

public enum Status 
{ 
    Preparing, 
    EnRoute, 
    Delivered, 
    Cancelled 
}

public partial class Order : BaseEntity
{
    [Column(Order = 1)]
    public string OrderNo { get; set; } = null!;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Firstname { get; set; } = null!;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Lastname { get; set; } = null!;

    [Phone]
    [Required]
    public string Tel { get; set; } = null!;

    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    public Address DeliveryAddress { get; set; } = null!;

    public decimal Total {get; set; }

    public Status Status {get; set; } = Status.Preparing;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    public Order() => OrderDetails = new HashSet<OrderDetail>();
}