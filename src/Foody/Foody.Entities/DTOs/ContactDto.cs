#nullable disable

namespace Foody.Entities.DTOs;

public class ContactDto
{
    [Required]
    public string Name { get; set; }

    public string Tel { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Subject { get; set; }

    public string Message { get; set; }
}

