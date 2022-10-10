namespace Foody.Entities.Models;

public sealed record Newsletter : IEquatable<Newsletter>, IEntity
{
    [Key]
    public int Id { get; init; }

    [StringLength(50, MinimumLength = 3)]
    public string? Name { get; init; }

    [Required]
    [EmailAddress]
    public string Email { get; init; } = null!;

    public bool Equals(Newsletter? other) => Email == other!.Email;

    public override int GetHashCode() => Email.GetHashCode();

}

