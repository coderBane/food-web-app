namespace Foody.Admin.Models;

public class Inquiry : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Tel { get; set; }

    public string Email { get; set; }

    public string Subject { get; set; }

    public string Message { get; set; }

    public DateTime Date { get; set; }

    public bool Flagged { get; set; }
}

