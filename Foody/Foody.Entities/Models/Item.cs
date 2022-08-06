global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;

namespace Foody.Entities.Models;

public class Item
{
    [Required]
    [StringLength(50, MinimumLength = 4)]
    public string Name { get; set; } = null!;

    [Display(Name = "State")]
    public  bool IsActive { get; set; }

    public string ImageUri { get; set; } = "";

    [NotMapped]
    public byte[] ImageData { get; set; } = Array.Empty<byte>();
}

