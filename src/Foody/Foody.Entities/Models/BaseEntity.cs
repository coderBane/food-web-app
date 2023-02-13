namespace Foody.Entities.Models;

public interface IEntity
{
    public int Id { get; init;}
}

public abstract class BaseEntity : IEntity
{
    [Key]
    [Column(Order = 0)]
    public int Id { get; init; }

    [ScaffoldColumn(false)]
    public int State { get; set; } = 1;

    [ScaffoldColumn(false)]
    public DateTime AddedOn { get; set; } = DateTime.UtcNow;

    [ScaffoldColumn(false)]
    public DateTime Updated { get; set; } = default(DateTime);
}