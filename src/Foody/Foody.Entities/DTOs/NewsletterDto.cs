using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Foody.Entities.DTOs;

public class NewsletterDto
{
    [AllowNull]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }
}

