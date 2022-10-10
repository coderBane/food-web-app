namespace Foody.Entities.Models
{
    public sealed class AppFile : BaseEntity
    {
        [MaxLength(1048576, ErrorMessage = "Max upload size is 1MB.")]
        public byte[] Content { get; set; } = null!;

        public string UntrustedName { get; set; } = null!;

        [MaxLength(100)]
        public string? Description { get; set; }

        [FileExtensions(Extensions = "png,jpg,jpeg,svg")]
        public string FileExtension { get; set; } = null!;

        public decimal Size { get; set; }
    }
}

