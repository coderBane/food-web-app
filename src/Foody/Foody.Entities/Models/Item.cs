namespace Foody.Entities.Models;

public abstract class Item : BaseEntity
{
    [Required]
    [StringLength(50, MinimumLength = 4)]
    public string Name { get; set; } = null!;

    [Display(Name = "State")]
    public  bool IsActive { get; set; }

    [ScaffoldColumn(false)]
    public string ImageUri { get; set; } = "images/noImg.jpeg";

    [NotMapped]
    [MaxLength(3145728, ErrorMessage = "Max upload size is 3MB.")]
    public byte[] ImageData { get; set; } = Array.Empty<byte>();
}

