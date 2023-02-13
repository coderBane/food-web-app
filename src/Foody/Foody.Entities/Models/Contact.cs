namespace Foody.Entities.Models;

public sealed class Contact : IEntity
{
    [Key]
    public int Id { get; init; }

    public bool Read { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Phone]
    public string? Tel { get; set; }

    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 10)]
    public string Subject { get; set; } = null!;

    [DataType(DataType.MultilineText)]
    public string? Message { get; set; }

    [ScaffoldColumn(false)]
    public DateTime Date { get; set; } = DateTime.UtcNow; 
}

