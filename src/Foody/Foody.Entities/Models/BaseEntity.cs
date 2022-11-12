namespace Foody.Entities.Models;

public interface IEntity
{
    
}

public abstract class BaseEntity : IEntity
{
    [Key]
    [Column(Order = 0)]
    public int Id { get; set; }

    [ScaffoldColumn(false)]
    public int State { get; set; } = 1;

    [ScaffoldColumn(false)]
    public DateTime AddedOn { get; set; } = DateTime.UtcNow;

    [ScaffoldColumn(false)]
    public DateTime Updated { get; set; }  = default(DateTime);
}