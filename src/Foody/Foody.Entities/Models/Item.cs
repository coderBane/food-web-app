namespace Foody.Entities.Models;

public abstract class Item : BaseEntity
{
    [Required]
    [Column(Order = 1)]
    [StringLength(50, MinimumLength = 4)]
    public string Name { get; set; } = null!;

    [Display(Name = "Status")]
    public  bool IsActive { get; set; }

    [ScaffoldColumn(false)]
    public string ImageUri { get; set; }

    [ScaffoldColumn(false)]
    public AppFile Image { get; set; }

    public Item()
    {
        Image = new AppFile
        {
            Content = File.ReadAllBytes("wwwroot/images/noImg.jpeg"),
            UntrustedName = "noImg.jpeg",
            FileExtension = ".jpeg",
            Description = "Default Image"
        };

        Image.Size = Image.Content.Length;

        ImageUri = (Guid.NewGuid() + "-" + Image.UntrustedName).Replace(" ", "");
    }

    //[MaxLength(2097152, ErrorMessage = "Max upload size is 2MB.")]
    //public byte[] ImageData { get; set; } = Array.Empty<byte>();
}

