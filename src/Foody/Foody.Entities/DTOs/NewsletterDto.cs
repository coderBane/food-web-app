namespace Foody.Entities.DTOs;

public class NewsletterDto
{
    public string? Name { get; set; }

    [Required]
    public string Email { get; set; } = null!;
}

