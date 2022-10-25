namespace Foody.Admin.Models;

public interface IEntity
{
    int Id { get; set; }
}

public abstract class Base : IEntity
{
    internal JsonSerializerOptions _options;

    public int Id { get; set; }

    public DateTime AddedOn { get; set; }

    public DateTime Updated { get; set; }

    public Base()
    {
        _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
    }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, _options);
    }
}

