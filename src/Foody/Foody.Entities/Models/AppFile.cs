namespace Foody.Entities.Models
{
    public abstract class ImageFile : BaseEntity
    {
        [MaxLength(2097152, ErrorMessage = "Max upload size is 2MB.")]
        public byte[] Content { get; set; } = null!;

        public string UntrustedName { get; set; } = null!;

        [MaxLength(100)]
        public string? Description { get; set; }

        [FileExtensions(Extensions = "png,jpg,jpeg,svg")]
        public string FileExtension { get; set; } = null!;

        public long Size { get; set; }
    }

    public sealed class ProductImage : ImageFile
    {
        public int ItemId {get; set; }

        public Item? Item { get; set; }
    }
}
