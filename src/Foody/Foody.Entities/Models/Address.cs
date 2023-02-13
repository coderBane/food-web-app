namespace Foody.Entities.Models;

public sealed class Address
{
    [Required]
    public string Line1 { get; set; } = null!;

    public string? Line2 { get; set; }

    [Required]
    public string City { get; set; } = null!;

    [Required]
    [MaxLength(8)]
    [RegularExpression(@"(\w{1,2}\d{1,2})\s*(\d\w{2})", ErrorMessage = "Invalid PostCode")]
    public string PostCode { get; set; } = null!;

    [ReadOnly(true)]
    public string Country { get; set; } = "United Kingdom";
}