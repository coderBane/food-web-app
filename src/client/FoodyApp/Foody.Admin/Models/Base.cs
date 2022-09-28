namespace Foody.Admin.Models;

public abstract class Base
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

