namespace Foody.Entities.DTOs;

public class EmailDto
{
    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public string From { get; set; } = string.Empty;

    public string To { get; set; } = string.Empty;
}

