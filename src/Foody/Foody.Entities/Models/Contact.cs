namespace Foody.Entities.Models;

public sealed record Contact : IEntity
{
    [Key]
    public int Id { get; init; }

    [Required]
    [MaxLength(50)]
    public string Name { get; init; } = null!;

    [Phone]
    public string? Tel { get; init; }

    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; init; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 10)]
    public string Subject { get; init; } = null!;

    [DataType(DataType.MultilineText)]
    public string? Message { get; init; }

    [ScaffoldColumn(false)]
    public DateTime Date { get; init; } = DateTime.UtcNow; 
}

