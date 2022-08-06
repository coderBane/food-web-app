namespace Foody.Entities.Models;

public abstract class Item : BaseEntity
{
    [Required]
    [Column(Order = 1)]
    [StringLength(50, MinimumLength = 4)]
    public string Name { get; set; } = null!;

    [Column(Order = 2)]
    [Display(Name = "State")]
    public bool IsActive { get; set; }

    [ScaffoldColumn(false)]
    [DataType(DataType.ImageUrl)]
    public string? ImageUri { get; set; } = "";

    [NotMapped]
    public byte[] ImageData { get; set; } = Array.Empty<byte>();

}

