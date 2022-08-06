namespace Foody.Entities.Models;

public abstract class Item : BaseEntity
{
    [Required]
    [StringLength(50, MinimumLength = 4)]
    public string Name { get; set; } = null!;

    [Display(Name = "State")]
    public  bool IsActive { get; set; }

    [ScaffoldColumn(false)]
    public string ImageUri { get; set; } = "";

    [NotMapped]
    public byte[] ImageData { get; set; } = Array.Empty<byte>();
}

