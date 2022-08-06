namespace Foody.Entities.Models;

public abstract class BaseEntity
{
    public int Id { get; set; }

    public int Status { get; set; } = 1;

    public DateTime AddedOn { get; set; } = DateTime.UtcNow;

    public DateTime Updated { get; set; }
}